using Microsoft.Extensions.Logging;
using Rongbo.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rongbo.Core.Service
{
    public abstract class ServiceBase : IService
    {
        private ILogger _logger;

        /// <summary>
        /// 日志记录
        /// </summary>
        protected ILogger Logger
        {
            get
            {
                if (_logger == null)
                    _logger = Log.Create(GetType().FullName);
                return _logger;
            }
        }
    }
}
