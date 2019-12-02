using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Rongbo.Core
{
    public interface IQuery<TEntity, TKey> : IQuery<TEntity> where TEntity : IBaseEntity
    {
        Task<List<TEntity>> GetAsync(IEnumerable<TKey> ids);
    }

    public interface IQuery<TEntity> where TEntity : IBaseEntity
    {
        Task<TEntity> GetAsync(params object[] keys);

        Task<TDto> GetAsync<TDto>(params object[] keys);

        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> @where = null, params Expression<Func<TEntity, object>>[] includes);

        Task<TDto> GetAsync<TDto>(Expression<Func<TEntity, bool>> @where = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        Task<IList<TEntity>> GetListAsync(int count = 0, Expression<Func<TEntity, bool>> @where = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);


        Task<IList<TDto>> GetListAsync<TDto>(int count = 0, Expression<Func<TEntity, bool>> @where = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        Task<bool> ExistAsync(params object[] keys);

        Task<bool> ExistAsync(Expression<Func<TEntity, bool>> @where = null);

        Task<int> CountAsync(Expression<Func<TEntity, bool>> @where = null);

        IQueryable<TEntity> AsQueryable();
    }
}
