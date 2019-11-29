using Rongbo.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rongbo.Entity
{
    public class Category : IBaseEntity<int>
    {
        public int Id { get; set; }

        public string CategoryName { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}
