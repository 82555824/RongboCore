using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Rongbo.Common.Utilities;
using Rongbo.Core.DependencyInjection;
using Rongbo.Core.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Rongbo.Core
{
    public abstract class UnitOfWorkBase : DbContext, IUnitOfWork, IUnitOfWorkContext
    {

        private readonly Dictionary<Type, IRepository> _repositories = new Dictionary<Type, IRepository>();

        protected UnitOfWorkBase(DbContextOptions options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var loggerFactory = Log.GetLoggerFactory();
            if (loggerFactory != null)
                optionsBuilder.UseLoggerFactory(loggerFactory);
        }

        public async Task<int> CommitAsync()
        {
            return await SaveChangesAsync();
        }


        public virtual async Task LoadCollectionAsync<T, TProperty>(T entry, Expression<Func<T, IEnumerable<TProperty>>> propertyExpression)
            where T : class
            where TProperty : class
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNull(propertyExpression, nameof(propertyExpression));

            await Entry(entry).Collection(propertyExpression).LoadAsync();
        }


        public virtual async Task LoadReferenceAsync<T, TProperty>(T entry, Expression<Func<T, TProperty>> propertyExpression)
            where T : class
            where TProperty : class
        {
            Check.NotNull(entry, nameof(entry));
            Check.NotNull(propertyExpression, nameof(propertyExpression));

            await Entry(entry).Reference(propertyExpression).LoadAsync();
        }

        public IQueryable<TProperty> QueryCollection<T, TProperty>(T entry, Expression<Func<T, IEnumerable<TProperty>>> propertyExpression)
            where T : class
            where TProperty : class
        {
            Check.NotNull(propertyExpression, nameof(propertyExpression));

            return Entry(entry).Collection(propertyExpression).Query();
        }


        public virtual TRepository GetDefineRepository<TRepository>() where TRepository : class, IRepository
        {
            return ContextAccessor.GetHttpContext().RequestServices.GetService<TRepository>();
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IBaseEntity
        {
            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
            {
                var repo = new RepositoryBase<TEntity>(this);
                _repositories[type] = repo;
                return repo;
            }
            return _repositories[type] as IRepository<TEntity>;
        }

        protected virtual bool MapTypeFilter(Type type)
        {
            return true;
        }

        protected virtual bool QueryTypeFilter(Type type)
        {
            return true;
        }

        protected virtual IEnumerable<Type> GetMapTypes(Assembly assembly)
        {
            return Reflection.GetTypesByInterface<IDbTypeMap>(assembly);
        }

        //protected virtual IEnumerable<Type> GetQueryTypes(Assembly assembly)
        //{
        //    return Reflection.GetTypesByInterface<IQueryEntity>(assembly);
        //}

    }
}
