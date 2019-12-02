using Microsoft.EntityFrameworkCore;
using Rongbo.Core;
using Rongbo.Entities.QueryEntities;
using Rongbo.Entity;
using Rongbo.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Rongbo.Repositories
{
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(IRongboUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<BookEntity> GetBook(int id)
        {
            return await Query<BookEntity>().FromSql($"select a.id,a.name,b.categoryname from tb_book a inner join tb_category b on a.categoryid = b.id where a.id={id}").FirstOrDefaultAsync();
        }
    }
}
