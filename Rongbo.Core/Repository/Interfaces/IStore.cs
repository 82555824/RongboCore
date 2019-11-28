using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Rongbo.Core
{
    /// <summary>
    /// 数据库操作 接口
    /// </summary>
    public interface IStore<TEntity, TKey> : IStore<TEntity> where TEntity : IBaseEntity
    {
        Task RemoveAsync(IEnumerable<TKey> ids);

    }

    public interface IStore<TEntity> where TEntity : IBaseEntity
    {
        Task AddAsync(TEntity entity);

        Task AddAsync(IEnumerable<TEntity> entities);

        void Update(TEntity entity);

        void Update(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);

        void Remove(IEnumerable<TEntity> entities);


        ValueTask Remove(object id);
    }
}
