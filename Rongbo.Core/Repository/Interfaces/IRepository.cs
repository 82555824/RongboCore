using System;
using System.Collections.Generic;
using System.Text;

namespace Rongbo.Core
{
    public interface IRepository { }

    public interface IRepository<TEntity> : IRepository, IStore<TEntity>, IQuery<TEntity> where TEntity : IBaseEntity
    {

    }

    public interface IRepository<TEntity, TKey> : IRepository<TEntity>, IStore<TEntity, TKey>, IQuery<TEntity, TKey> where TEntity : IBaseEntity<TKey>
    {

    }
}
