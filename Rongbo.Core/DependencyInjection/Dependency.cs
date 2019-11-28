using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rongbo.Core.DependencyInjection
{

    public class Dependency
    {
        private static Dependency _dependency;

        private Dependency()
        {
        }

        public static Dependency Instance
        {
            get
            {
                if (_dependency == null)
                    _dependency = new Dependency();
                return _dependency;
            }
        }

        public IContainer Container { get; internal set; }
    }

}
