using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Rongbo.Core
{
    public interface IUnitOfWorkReference
    {
        Task LoadReferenceAsync<T, TProperty>(T entry, Expression<Func<T, TProperty>> propertyExpression)
            where T : class where TProperty : class;


        Task LoadCollectionAsync<T, TProperty>(T entry,
            Expression<Func<T, IEnumerable<TProperty>>> propertyExpression) where T : class where TProperty : class;

        IQueryable<TProperty> QueryCollection<T, TProperty>(T entry,
        Expression<Func<T, IEnumerable<TProperty>>> propertyExpression) where T : class where TProperty : class;
    }
}
