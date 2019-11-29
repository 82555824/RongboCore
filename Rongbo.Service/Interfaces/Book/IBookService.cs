using Rongbo.Entity;
using Rongbo.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Rongbo.Service
{
    public interface IBookService
    {
        Task<BookModel> Get(int id);
    }
}
