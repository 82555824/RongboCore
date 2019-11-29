using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rongbo.Core.Mapping
{
    public interface IQueryDbTypeMap<TEntity> : IDbTypeMap where TEntity : class
    {
        void QueryDbTypeMapping(EntityTypeBuilder<TEntity> builder);
    }
}
