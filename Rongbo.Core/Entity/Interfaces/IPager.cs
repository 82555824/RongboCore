using System;
using System.Collections.Generic;
using System.Text;

namespace Rongbo.Core.Entity
{
    public interface IPager
    {
        int Count { get; set; }

        int Limit { get; set; }

        int Page { get; set; }
    }

    public interface IPager<TEntity> : IPager
    {
        List<TEntity> List { get; set; }

        IPager<TResult> Select<TResult>(Func<TEntity, TResult> selector);

        IPager<TResult> Map<TResult>();
    }
}
