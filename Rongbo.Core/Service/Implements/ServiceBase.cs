using Microsoft.Extensions.Logging;
using Rongbo.Common.Utilities;
using Rongbo.Core.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Rongbo.Core.Service
{
    public abstract class ServiceBase<TEntity> : IService where TEntity : class, IBaseEntity
    {
        private ILogger _logger;

        protected readonly IUnitOfWork _unitOfWork;

        public ServiceBase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

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

        protected IRepository<TEntity> repository => _unitOfWork.GetRepository<TEntity>();
    }
}
