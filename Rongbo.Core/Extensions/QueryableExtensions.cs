using Autofac;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Rongbo.Common.Utilities;
using Rongbo.Core.DependencyInjection;
using Rongbo.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rongbo.Core.Extensions
{
    public static class QueryableExtensions
    {

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string field)
        {
            var prop = TypeDescriptor.GetProperties(typeof(T)).Find(field, true);
            Check.NotNull(prop, nameof(prop));

            return query.OrderBy(o => prop.GetValue(o));
        }

        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> query, string field)
        {
            var prop = TypeDescriptor.GetProperties(typeof(T)).Find(field, true);
            Check.NotNull(prop, nameof(prop));

            return query.ThenBy(o => prop.GetValue(o));
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> query, string field)
        {
            var prop = TypeDescriptor.GetProperties(typeof(T)).Find(field, true);
            Check.NotNull(prop, nameof(prop));

            return query.OrderByDescending(o => prop.GetValue(o));
        }

        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> query, string field)
        {
            var prop = TypeDescriptor.GetProperties(typeof(T)).Find(field, true);
            Check.NotNull(prop, nameof(prop));

            return query.ThenByDescending(o => prop.GetValue(o));
        }

        public static async Task<IPager<T>> GetPagerAsync<T>(this IQueryable<T> queryable, int page, int limit) where T : class
        {
            var pager = new Pager<T>();
            pager.Page = page;
            pager.Limit = limit;
            pager.Count = await queryable.CountAsync();
            if (pager.Count > 0)
                pager.List = await queryable.Skip((pager.Page - 1) * pager.Limit).Take(pager.Limit).AsNoTracking().ToListAsync();
            return pager;
        }


        public static async Task<IPager<TDto>> GetPagerAsync<TDto>(this IQueryable queryable, int page, int limit) where TDto : class
        {
            var _mapper = Dependency.Instance.Container.ResolveOptional<IMapper>();
            return await GetPagerAsync(queryable.ProjectTo<TDto>(_mapper.ConfigurationProvider), page, limit);
        }

        public static async Task<IList<T>> GetLimitAsync<T>(this IQueryable<T> queryable, int limit, int size) where T : class
        {
            return await queryable.Skip(limit).Take(size).AsNoTracking().ToListAsync();
        }

        public static async Task<IList<TDto>> GetLimitAsync<TDto>(this IQueryable queryable, int limit, int size) where TDto : class
        {
            var _mapper = Dependency.Instance.Container.ResolveOptional<IMapper>();
            return await GetLimitAsync(queryable.ProjectTo<TDto>(_mapper.ConfigurationProvider), limit, size);
        }

        public static IQueryable<T> Select<T>(this IQueryable queryable) where T : class
        {
            var _mapper = Dependency.Instance.Container.ResolveOptional<IMapper>();
            return queryable.ProjectTo<T>(_mapper.ConfigurationProvider);
        }
    }
}
