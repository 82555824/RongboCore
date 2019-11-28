using System;
using System.Collections.Generic;
using System.Text;

namespace Rongbo.Core
{
    public interface IBaseEntity
    {
    }

    public interface IBaseEntity<Tkey> : IBaseEntity
    {
        Tkey Id { get; set; }
    }

}
