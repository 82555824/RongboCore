using Rongbo.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rongbo.Common.AutoMapper
{
    public class MapperRegister
    {
        public static Type[] MapType()
        {
            Type[] types = Reflection.GetTypesByInterface<IProfile>().ToArray();
            return types;
        }
    }
}
