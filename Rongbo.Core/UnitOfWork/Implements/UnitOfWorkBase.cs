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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            MapEntities(modelBuilder);
        }

        protected virtual void MapEntities(ModelBuilder modelBuilder)
        {
            var entityMethodInfo = modelBuilder.GetType().GetMethod("Entity", new Type[] { });
            //var queryMethodInfo = modelBuilder.GetType().GetMethod("Query", new Type[] { });
            //var registereds = new List<Type>();
            foreach (var assembly in GetMapAssemblies())
            {
                foreach (var mapType in GetMapTypes(assembly))
                {
                    if (MapTypeFilter(mapType))
                    {
                        dynamic map = Dependency.Instance.Container.ResolveOptional(mapType);
                        if (map != null)
                        {
                            Type type = null;
                            if ((type = mapType.GetInterface("IEntityDbTypeMap`1")) != null)
                            {
                                var entityType = type.GenericTypeArguments.First();
                                var methodInfo = entityMethodInfo.MakeGenericMethod(entityType);
                                dynamic builder = methodInfo.Invoke(modelBuilder, null);
                                map.EntityDbTypeMapping(builder);
                            }
                            //if ((type = mapType.GetInterface("IQueryDbTypeMap`1")) != null)
                            //{
                            //    var queryType = type.GenericTypeArguments.First();
                            //    var methodInfo = queryMethodInfo.MakeGenericMethod(queryType);
                            //    dynamic builder = methodInfo.Invoke(modelBuilder, null);
                            //    map.QueryDbTypeMapping(builder);
                            //    registereds.Add(queryType);
                            //}
                        }
                    }
                }
            }

            //var queryTypes = GetMapAssemblies().SelectMany(GetQueryTypes).Where(o => !registereds.Contains(o)).Where(QueryTypeFilter);
            //foreach (var queryType in queryTypes)
            //    queryMethodInfo.MakeGenericMethod(queryType).Invoke(modelBuilder, null);
        }

        protected virtual Assembly[] GetMapAssemblies()
        {
            return Reflection.Assemblies.ToArray();
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
