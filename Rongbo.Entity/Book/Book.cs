using Rongbo.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rongbo.Entity
{
    public class Book : IBaseEntity<int>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CategoryId { get; set; }

        public decimal Price { get; set; }

        public Category Category { get; set; }
    }
}
