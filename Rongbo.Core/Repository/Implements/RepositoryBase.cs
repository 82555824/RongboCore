using Autofac;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Rongbo.Common.Utilities;
using Rongbo.Core.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Rongbo.Core
{
    public class RepositoryBase<TEntity, TKey> : RepositoryBase<TEntity>, IRepository<TEntity, TKey> where TEntity : class, IBaseEntity<TKey>
    {
        public RepositoryBase(IUnitOfWork context) : base(context)
        {
        }

        public async Task<List<TEntity>> GetAsync(IEnumerable<TKey> ids)
        {
            return await AsQueryable().Where(o => ids.Contains(o.Id)).ToListAsync();
        }

        public async Task RemoveAsync(IEnumerable<TKey> ids)
        {
            var ls = await GetAsync(ids);
            Remove(ls);
        }
    }

    public class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class, IBaseEntity
    {
        private IReadOnlyList<IProperty> _properties;

        private IUnitOfWorkContext _context;

        protected DbSet<TEntity> Table { get; }

        protected DatabaseFacade Database { get; }

        private IMapper _mapper;

        public RepositoryBase(IUnitOfWork context)
        {
            _context = context as IUnitOfWorkContext;
            Table = _context.Set<TEntity>();
            Database = context.Database;
            _mapper = Dependency.Instance.Container.Resolve<IMapper>();
        }

        public async Task AddAsync(TEntity entity)
        {
            await Table.AddAsync(entity);
        }

        public async Task AddAsync(IEnumerable<TEntity> entities)
        {
            await Table.AddRangeAsync(entities);
        }

        public IQueryable<TEntity> AsQueryable()
        {
            return Table.AsQueryable();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> where = null)
        {
            if (where != null)
                return await Table.CountAsync(where);
            return await Table.CountAsync();
        }

        public async Task<bool> ExistAsync(Expression<Func<TEntity, bool>> where = null)
        {
            if (where != null)
                return await Table.AnyAsync(where);
            return await Table.AnyAsync();
        }

        public async Task<bool> ExistAsync(params object[] keys)
        {
            Check.NotEmpty(keys, nameof(keys));

            return await ExistAsync(GetPrimaryKeyPredicate(keys));
        }

        

        protected Expression<Func<TEntity, bool>> GetPrimaryKeyPredicate(object[] keys)
        {
            if (keys == null || keys.Length == 0)
                return default;

            if (_properties == null)
                _properties = _context.Model.FindEntityType(typeof(TEntity)).FindPrimaryKey().Properties;
            var parameter = Expression.Parameter(typeof(TEntity), "e");
            var body = _properties
                .Select((p, i) => Expression.Equal(
                    Expression.Property(parameter, p.Name),
                    Expression.Convert(
                        Expression.PropertyOrField(Expression.Constant(new { id = keys[i] }), "id"),
                        p.ClrType)))
                .Aggregate(Expression.AndAlso);
            return Expression.Lambda<Func<TEntity, bool>>(body, parameter);
        }

        public void Update(TEntity entity)
        {
            Table.Update(entity);
        }

        public void Update(IEnumerable<TEntity> entities)
        {
            Table.UpdateRange(entities);
        }

        public void Remove(TEntity entity)
        {
            Table.Remove(entity);
        }

        public void Remove(IEnumerable<TEntity> entities)
        {
            Table.RemoveRange(entities);
        }

        public async ValueTask<TEntity> GetAsync(params object[] keys)
        {
            Check.NotEmpty(keys, nameof(keys));

            return await Table.FindAsync(keys);
        }

        public async Task<TDto> GetAsync<TDto>(params object[] keys)
        {
            Check.NotEmpty(keys, nameof(keys));

            return await Table.Where(GetPrimaryKeyPredicate(keys)).ProjectTo<TDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> where = null, params Expression<Func<TEntity, object>>[] includes)
        {
            var queryable = AsQueryable();
            foreach (var include in includes)
                queryable = queryable.Include(include);
            return await queryable.FirstOrDefaultAsync(where);
        }

        public async Task<TDto> GetAsync<TDto>(Expression<Func<TEntity, bool>> where = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            var queryable = AsQueryable();
            if (where != null)
                queryable = queryable.Where(where);
            if (orderBy != null)
                queryable = orderBy(queryable);
            return await queryable.ProjectTo<TDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
        }

        public async Task<IList<TEntity>> GetListAsync(int count = 0, Expression<Func<TEntity, bool>> where = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            var queryable = AsQueryable();
            if (where != null)
                queryable = queryable.Where(where);
            if (count > 0)
                queryable = queryable.Take(count);
            if (orderBy != null)
                queryable = orderBy(queryable);

            return await queryable.ToListAsync();
        }

        public async Task<IList<TDto>> GetListAsync<TDto>(int count = 0, Expression<Func<TEntity, bool>> where = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            var queryable = AsQueryable();
            if (where != null)
                queryable = queryable.Where(where);
            if (count > 0)
                queryable = queryable.Take(count);
            if (orderBy != null)
                queryable = orderBy(queryable);

            return await queryable.ProjectTo<TDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async ValueTask Remove(object id)
        {
            var entity = await GetAsync(id);
            Remove(entity);
        }
    }

}
