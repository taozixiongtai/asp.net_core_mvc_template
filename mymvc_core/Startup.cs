using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mymvc_core.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using mymvc_core.Models;
using mymvc_core.Services.Email.MimeKit;
using mymvc_core.Services.Email;
using mymvc_core.Services.interfaces;
using mymvc_core.Services;
using mymvc_core.Hubs;
namespace mymvc_core
{
    public class Startup
    {


        public Startup(IConfiguration configuration)        //这就是依赖注入，我来说明一下，这个类他需要一个配置（configuration），但是他不自己去创建，而是使用构造函数去让别人给他创建，又因为设计模式的问题，他定义了接口方便别人去调用，这就是依赖注入
        {
            Configuration = configuration;      //这个是去配置文件里面读取配置。
        }
        public IConfiguration Configuration { get; }





        // 使用这个方法去添加服务到容器里面,所以你可以在这里面看到很多添加方法，然后可以在Configure方法里面去调用
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddSignalR();//这是我写的聊天室的。
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(Configuration.GetConnectionString("MysqlConnection")));
            services.AddDbContext<CoremvcContext>(options => options.UseMySql(Configuration.GetConnectionString("MysqlConnection")));//测试第三方框架的时候为了注入加的服务
            services.AddScoped<IMessageServer, MessageServer>();//这是自己添加的组件，这个组件是用来和EF完成一系列CURD的。
            #region 登录验证的配置
            services.Configure<IdentityOptions>(options =>
            {
                // 密码设置
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 6;

                // 登出设置
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // 用户设置
                options.User.RequireUniqueEmail = false;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie 设置
                options.Cookie.HttpOnly = true;
                options.Cookie.Expiration = TimeSpan.FromDays(150);

                // 未授权的登录页面.
                options.LoginPath = "/identity/ExternalLogin";

                // 登录后，但是没有权限访问这个页面，就会被定位到这里
                options.AccessDeniedPath = "/Account/AccessDenied";
                //更多设置请通过元数据去查看。
            });
            #endregion
            services.AddTransient<IEmailSender, EmailSender>();    //发送邮件服务
            services.AddAuthentication().AddMicrosoftAccount(microsoftOptions =>    //微软的第三方登录。
            {
                microsoftOptions.ClientId = "7a0c4b2a-1990-45af-8a97-9e66cc4bce36";
                microsoftOptions.ClientSecret = "anxUDGJM987*|rjsuKM94):";
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

        }






        // 使用这方法去配置HTTP请求管道，这里全是中间件
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())   //开发模式
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {

                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days.You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/Home/Error/", "?code={0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();                        //这是身份验证的。
            app.UseSignalR(routes =>                       //这是聊天室的路由
            {
                routes.MapHub<ChatHub>("/chatHub");
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

           

        }

      
    }
}
