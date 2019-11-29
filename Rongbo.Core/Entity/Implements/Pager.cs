using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rongbo.Common.AutoMapper;

namespace Rongbo.Core.Entity
{
    public class Pager<TEntity> : IPager<TEntity>
    {
        public List<TEntity> List { get ; set ; }
        public int Count { get ; set ; }
        public int Limit { get ; set ; }
        public int Page { get; set ; }

        public IPager<TResult> Map<TResult>()
        {
            var pager = new Pager<TResult>();
            pager.Page = Page;
            pager.Limit = Limit;
            pager.Count = Count;
            pager.List = List.MapToList<TResult>();
            return pager;
        }

        public IPager<TResult> Select<TResult>(Func<TEntity, TResult> selector)
        {
            var pager = new Pager<TResult>();
            pager.Page = Page;
            pager.Limit = Limit;
            pager.Count = Count;
            pager.List = List?.Select(selector).ToList();
            return pager;
        }
    }
}
