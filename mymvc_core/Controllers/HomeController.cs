using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mymvc_core.Models;
using mymvc_core.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using mymvc_core.Extensions;

namespace mymvc_core.Controllers
{
    public class HomeController : Controller
    {


        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(
            UserManager<ApplicationUser> userManager

            )
        {
            _userManager = userManager;

        }
        public async Task< IActionResult> Index()
        {
            ClaimsIdentity c = HttpContext.User.Identity as ClaimsIdentity;
            if (HttpContext.User.Identity is ClaimsIdentity)
            {
                ViewBag.a = "可以转换";
            }
            if (c == null)
            {
                ViewBag.b = "但是没有值";
            }
            else
            {
                ViewBag.b = "里面有值";
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                ViewBag.c = "没能获取到当前登录的用户";
            }
           
          
            return View(c.Claims);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int code)
        {

            ViewBag.a = code;//状态码
            return View();
        }
    }
}
