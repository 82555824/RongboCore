using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Rongbo.Core
{
    public interface IUnitOfWorkCommit
    {
        Task<int> CommitAsync();
    }
}
