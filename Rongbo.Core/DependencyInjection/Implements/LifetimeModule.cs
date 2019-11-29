using Autofac;
using Microsoft.Extensions.Logging;
using Rongbo.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rongbo.Core.DependencyInjection
{
    public class LifetimeModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //设置日志工厂
            builder.RegisterBuildCallback(container => Log.SetLoggerFactory(container.ResolveOptional<ILoggerFactory>()));

            //自动注册
            Type[] types;
            types = Reflection.GetTypesByInterface<IRegisterSingletonAsSelf>().ToArray();
            if (types.Length > 0)
                builder.RegisterTypes(types).AsSelf().AsImplementedInterfaces().SingleInstance();

            types = Reflection.GetTypesByInterface<IRegisterTransientAsSelf>().ToArray();
            if (types.Length > 0)
                builder.RegisterTypes(types).AsSelf().AsImplementedInterfaces().InstancePerDependency();

            types = Reflection.GetTypesByInterface<IRegisterScopedAsSelf>().ToArray();
            if (types.Length > 0)
                builder.RegisterTypes(types).AsSelf().AsImplementedInterfaces().InstancePerLifetimeScope();

            types = Reflection.GetTypesByInterface<IRegisterSingleton>().ToArray();
            if (types.Length > 0)
                builder.RegisterTypes(types).AsImplementedInterfaces().SingleInstance();

            types = Reflection.GetTypesByInterface<IRegisterTransient>().ToArray();
            if (types.Length > 0)
                builder.RegisterTypes(types).AsImplementedInterfaces().InstancePerDependency();

            types = Reflection.GetTypesByInterface<IRegisterScoped>().ToArray();
            if (types.Length > 0)
                builder.RegisterTypes(types).AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}
