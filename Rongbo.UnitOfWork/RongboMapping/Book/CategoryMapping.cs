using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rongbo.Core.Mapping;
using Rongbo.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rongbo.UnitOfWork.RongboMapping
{
    public class CategoryMapping : IEntityDbTypeMap<Category>
    {
        public void EntityDbTypeMapping(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(o => o.Id);
            builder.ToTable("tb_category");
        }
    }
}
