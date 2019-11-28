using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rongbo.Core
{
    public interface IUnitOfWorkContext
    {
        DbQuery<TQuery> Query<TQuery>() where TQuery : class;

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        EntityEntry Entry(object entity);

        IModel Model { get; }
    }
}
