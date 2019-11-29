using System;
using System.Collections.Generic;
using System.Text;

namespace Rongbo.Model.RequestModels
{
    public class BookRequest
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CatgeoryId { get; set; }
    }
}
