using Rongbo.Core;
using Rongbo.Entities.QueryEntities;
using System.Threading.Tasks;

namespace Rongbo.Repositories
{
    public interface IBookRepository : IRepository
    {
        Task<BookEntity> GetBook(int id);
    }
}
