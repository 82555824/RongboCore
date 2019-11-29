using Rongbo.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rongbo.Entities.QueryEntities
{
    public class BookEntity : IQueryEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string CategoryName { get; set; }
    }
}
