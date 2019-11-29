using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Rongbo.Common.AutoMapper;
using Rongbo.Common.Models;
using Rongbo.Common.Utilities;
using Rongbo.Core;
using Rongbo.Core.DependencyInjection;
using Rongbo.UnitOfWork;
using Rongbo.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Rongbo.Common.NLog;
using Rongbo.Common.Settings;

namespace RongboMvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<Authentication>(Configuration.GetSection("Authentication"));
            //cookie 身份验证
            services.AddAuthentication(Configuration["Authentication:CookieAuthenticationScheme"])
                .AddCookie(Configuration["Authentication:CookieAuthenticationScheme"], options =>
                {
                    options.LoginPath = "/home/login";
                    options.ReturnUrlParameter = "returnurl";
                    options.Cookie.Name = Configuration["Authentication:CookieName"];
                    options.Cookie.Domain = Configuration["Authentication:CookieDomain"];
                });

            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddUnitOfWork<IRongboUnitOfWork, RongboUnitOfWork>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("MySqlServer"),
                    op =>
                    {
                        op.UseRowNumberForPaging();
                    });
            });
            services.AddAutoMapper(MapperRegister.MapType());

            services.AddNLog();

            services.AddControllersWithViews().AddNewtonsoftJson(options => {
                //忽略循环引用
                options.SerializerSettings.ReferenceLoopHandling = GlobalSettings.JsonSettings.ReferenceLoopHandling;
                //不使用驼峰样式的key
                options.SerializerSettings.ContractResolver = GlobalSettings.JsonSettings.ContractResolver;
                //设置时间格式
                options.SerializerSettings.DateFormatString = GlobalSettings.JsonSettings.DateFormatString;

                options.SerializerSettings.ObjectCreationHandling = GlobalSettings.JsonSettings.ObjectCreationHandling;
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            //自动注册自定义仓储
            var types = Reflection.GetTypesByInterface<IRepository>().Where(o => !o.IsGenericType);
            builder.RegisterTypes(types.ToArray()).AsImplementedInterfaces().InstancePerLifetimeScope();
            //自动注册服务
            types = Reflection.GetTypesByInterface<IService>().Where(o => !o.IsGenericType);
            builder.RegisterTypes(types.ToArray()).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterModule<LifetimeModule>();
            builder.RegisterBuildCallback(container =>
            {
                ContextAccessor._httpContextAccessor = container.Resolve<IHttpContextAccessor>();
                Dependency.Instance.Container = container;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseStateAutoMapper();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
