using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rongbo.Core.Extensions
{
    public static class UnitOfWorkExtensions
    {
        public static IServiceCollection AddUnitOfWork<TService, TImplementation>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction = null)
            where TService : class, IUnitOfWork
            where TImplementation : UnitOfWorkBase, TService
        {
            services.AddDbContext<TImplementation>(optionsAction);
            services.TryAddScoped<TService>(t => t.GetService<TImplementation>());
            return services;
        }
    }
}
