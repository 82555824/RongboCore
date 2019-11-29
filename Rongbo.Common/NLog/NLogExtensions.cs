using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.LayoutRenderers;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rongbo.Common.NLog
{
    public static class NLogExtensions
    {
        public static IServiceCollection AddNLog(this IServiceCollection services)
        {
            services.AddNLog(null);
            return services;
        }

        /// <summary>
        /// 添加NLog日志
        /// </summary>
        /// <param name="register"></param>
        public static IServiceCollection AddNLog(this IServiceCollection services, NLogAspNetCoreOptions options, bool clearProviders = true)
        {

            services.AddLogging(builder =>
            {
                if (clearProviders)
                    builder.ClearProviders();
                builder.SetMinimumLevel(LogLevel.Trace);
            });

            options = options ?? NLogAspNetCoreOptions.Default;

            services.AddSingleton<ILoggerProvider>(serviceProvider =>
            {
                serviceProvider.SetupNLogServiceLocator();
                if (options.RegisterHttpContextAccessor)
                {
                    LayoutRenderer.Register<WebInfoLayoutRenderer>("webinfo");
                    WebInfoLayoutRenderer.httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
                }
                return new NLogLoggerProvider(options);
            });
            if (options.RegisterHttpContextAccessor)
                services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            return services;
        }
    }
}
