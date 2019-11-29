using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rongbo.Core.Mapping;
using Rongbo.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rongbo.UnitOfWork.RongboMapping
{
    public class BookMapping : IEntityDbTypeMap<Book>
    {
        public void EntityDbTypeMapping(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(o => o.Id);
            builder.HasOne(o => o.Category).WithMany(o => o.Books).HasForeignKey(o => o.CategoryId);
            builder.ToTable("tb_book");
        }
    }
}
