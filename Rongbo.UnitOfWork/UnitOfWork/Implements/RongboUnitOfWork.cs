using Microsoft.EntityFrameworkCore;
using Rongbo.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rongbo.UnitOfWork
{
    public class RongboUnitOfWork : UnitOfWorkBase, IRongboUnitOfWork
    {
        public RongboUnitOfWork(DbContextOptions<RongboUnitOfWork> options) : base(options)
        {

        }

        protected override bool MapTypeFilter(Type type)
        {
            return type.FullName.StartsWith("Rongbo.UnitOfWork.RongboMapping");
        }
    }
}
