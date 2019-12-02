using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rongbo.Core
{
    public interface IUnitOfWork: IUnitOfWorkCommit, IUnitOfWorkReference
    {
        Task AddRangeAsync(params object[] entities);

        EntityEntry<TEntity> Attach<TEntity>(TEntity entity) where TEntity : class;

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        IRepository<T> GetRepository<T>() where T : class, IBaseEntity;

        TRepository GetDefineRepository<TRepository>() where TRepository : class, IRepository;

        DbQuery<TQuery> Query<TQuery>() where TQuery : class;

        DatabaseFacade Database { get; }

        ChangeTracker ChangeTracker { get; }
    }
}
