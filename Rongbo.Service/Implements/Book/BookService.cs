using Microsoft.EntityFrameworkCore;
using Rongbo.Core;
using Rongbo.Core.Service;
using Rongbo.Entity;
using Rongbo.Model.ViewModels;
using Rongbo.Repositories;
using Rongbo.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rongbo.Service
{
    public class BookService : ServiceBase<Book>, IBookService
    {

        public BookService(IRongboUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<BookModel> Get(int id)
        {
            var model = await repository.AsQueryable().AsNoTracking().Select(o=>new BookModel { Id = o.Id,Name = o.Name,CategoryName = o.Category.CategoryName}).FirstOrDefaultAsync(o => o.Id == id);

            var tt =  await _unitOfWork.GetDefineRepository<IBookRepository>().GetBook(id);
            return model;
        }
    }
}
