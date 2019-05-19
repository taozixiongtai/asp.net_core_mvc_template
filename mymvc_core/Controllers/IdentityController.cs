using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using mymvc_core.Models;
using mymvc_core.Models.ViewModels;
using mymvc_core.Data;
using mymvc_core.Services.Email;
using System.Security.Claims;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace mymvc_core.Controllers
{
    public class IdentityController : Controller
    {
        [TempData]
        public string ErrorMessage { get; set; }
        [TempData]
        public string StatusMessage { get; set; }



        private readonly IEmailSender _emailSender;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityController(
            UserManager<ApplicationUser> userManager,
             IEmailSender emailSender,
            SignInManager<ApplicationUser> signInManager
            , RoleManager<IdentityRole> roleManager

            )
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;

        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        public IActionResult Register()
        {



            using (var context = new ApplicationDbContext())
            {
                ViewBag.userList = context.Users.ToList();
            }

            return View();
        }




        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            ViewBag.NowUser = model.UserName;
            using (var context = new ApplicationDbContext())  //有延迟，需要修改。
            {
                ViewBag.userList = context.Users.ToList();
            }

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                    await _userManager.AddToRoleAsync(user, "Admin");

                    return View();
                }
            }

            return RedirectToAction("Register1");
        }


        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public IActionResult Login()
        {


            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    ViewBag.NowUser = model.UserName;
                    // return Content("<script>alert('登录成功');window.location.href='../Identity/Login';</script>");
                    return RedirectToAction("Register");

                }
                else
                {
                    return Content("<script>alert('失败，可能是密码错误');</script>;window.location.href='../Identity/Login';</script>");
                }
            }


            return View();
        }


        #region 第三方登录的3个方法




        public IActionResult ExternalLogin(string provider)
        {


            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "identity");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        public async Task<IActionResult> ExternalLoginCallback()
        {

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(ExternalLogin));
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {

                return RedirectToAction("Login");
            }

            if (result.IsLockedOut)
            {
                return RedirectToAction(nameof(Login));
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalLogin", new ExternalLoginViewModel { Email = email });
            }
        }

        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    throw new ApplicationException("Error loading external login information during confirmation.");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("index", "home");
                    }
                }
            }
            return View(nameof(ExternalLogin), model);
        }
        #endregion


        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(IdentityController.Register), "Identity");
        }

        //确定邮箱
        public IActionResult ConfirmEmail()
        {

            return View();

        }

        [HttpPost]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailViewModel model)
        {
            if (ModelState.IsValid)
            {



                var cc = model.userName;
                var user = await _userManager.FindByNameAsync(cc);

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);   //这里是拿到token
                var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme); //这是生成url，通过GET方法传入ID,TOKEN和协议方式。
                var email = model.emailAdress;//这里是要验证的邮箱，
                await _emailSender.SendEmailConfirmationAsync(email, callbackUrl);//发邮件过去
                user.Email = model.emailAdress;
                await _userManager.UpdateNormalizedEmailAsync(user);  //给用户添加邮箱。
                return View();
            }
            return View();

        }

        public async Task<IActionResult> ConfirmEmailResult(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var user = await _userManager.FindByIdAsync(userId);        //找到该用户
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }





        #region 登录测试的时候页面
        public IActionResult Login1(string returnUrl = null)
        {


            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        public IActionResult Register1()
        {


            return View();
        }
        #endregion





        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <returns></returns>
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    //如果没有该账号，或者该账号已经验证了邮箱。
                    return RedirectToAction(nameof(SetPassword));
                }
                //生成token
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);

                //await _emailSender.SendEmailAsync(model.Email, "Reset Password",
                //   $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");

                StatusMessage = "邮件已发送";
                ViewBag.A = callbackUrl;

                return Content(callbackUrl);

                //   return RedirectToAction(nameof(ForgotPassword));   //这里应该用一个弹窗去提示比较好。
            }
            return View(model);
        }


        public IActionResult SetPassword(string UserId, string code = null)//这应该是用邮件去登录的页面。
        {
            if (code == null)
            {
                throw new ApplicationException("请不要直接登录该页面。");
            }
            var model = new SetPasswordViewModel { Code = code, UserId = UserId };
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("index", "home");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.NewPassword);
            if (result.Succeeded)
            {
                return RedirectToAction("index", "home");
            }

            return View();
        }


        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ChangePassword()           
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                return RedirectToAction(nameof(SetPassword));
            }

            var model = new ChangePasswordViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {

                return View(model);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);


            return RedirectToAction(nameof(ChangePassword));
        }


        /// <summary>
        /// 添加角色
        /// </summary>
        /// <returns></returns>
        public IActionResult AddRole()
        {



            using (var context = new ApplicationDbContext())
            {
                ViewBag.roleList = context.Roles.ToList();


            }



            return View();



            //通过视图模型去返回一个集合。
            //var addroleViewModel = new AddRoleViewModel
            //{

            //    Users = new List<string>()
            //};
            //var users = context.Roles.ToList();
            //foreach (var user in users)
            //{

            //        addroleViewModel.Users.Add(user.Name);

            //}
            //return View(addroleViewModel);
        }

        /// <summary>
        /// 返回表单的添加角色方法
        /// </summary>
        /// <param name="addroleViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddRole(AddRoleViewModel addroleViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(addroleViewModel);
            }

            var role = new IdentityRole { Name = addroleViewModel.roleName };

            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("AddRole");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(addroleViewModel);//重复的问题没有解决
        }

        /// <summary>
        /// 给用户添加角色的方法
        /// </summary>
        /// <returns></returns>
        public  IActionResult EditRole()
        {
            var c = new UserRoleViewModel();

            using (var context = new ApplicationDbContext())
            {
               var roles= context.Roles.ToList();

                foreach (var item in roles)
                {
                    c.Roles.Add(item);
                }

                var users = context.Users.ToList();

                foreach (var item in users)
                {
                    c.Users.Add(item);
                }
            }

          

            return View(c);
        }

        /// <summary>
        /// 这是AJAX的方法
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>当前用户所拥有的角色</returns>
        public async Task< ActionResult> GetRolesList(string userName)
        {
            List<string> list = new List<string>();


            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);

            foreach (var item in roles)
            {
                list.Add(item);
            }
            return Content(JsonConvert.SerializeObject(list));
            //如果要使用这个方法去序列化需要引用NEWTONSOFT。dll,序列化后返回一个字符串
        }


        /// <summary>
        /// 这是poet方法的给用户添加角色
        /// </summary>
        /// <param name="userRoleViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> EditRole(UserRoleViewModel  userRoleViewModel)
        {
            var user = await _userManager.FindByIdAsync(userRoleViewModel.UserId);

            var role = await _roleManager.FindByIdAsync(userRoleViewModel.RoleId);

            if (user != null && role != null)
            {
                var result = await _userManager.AddToRoleAsync(user, role.Name);

                if (result.Succeeded)
                {
                    return RedirectToAction("EditRole");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(userRoleViewModel);
            }

            ModelState.AddModelError(string.Empty, "用户或角色未找到");
            return View(userRoleViewModel);
        }


        /// <summary>
        /// 这remote特性的验证方法
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> CheckRoleExist([Bind("roleName")]string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role != null)
            {                
                return Json(false);
            }

            return Json(true);
        }
    }
}