using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Rongbo.Common.Enum;
using Rongbo.Core.ApiResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RongboMvc.Filter
{
    public class GlobalExceptionsFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionsFilter> _loggerHelper;

        public GlobalExceptionsFilter(ILogger<GlobalExceptionsFilter> loggerHelper)
        {
            _loggerHelper = loggerHelper;
        }

        public void OnException(ExceptionContext context)
        {
            if (IsAjaxRequest(context.HttpContext))
            {
                context.ExceptionHandled = true;
                _loggerHelper.LogError(context.Exception, "ajax请求异常");
                context.Result = new RongboResult(StateCode.Fail, message: "系统繁忙");
            }
            else
            {
                _loggerHelper.LogError(context.Exception, "同步请求异常");
            }
        }

        protected bool IsAjaxRequest(HttpContext context)
        {
            var request = context.Request;
            if (!string.Equals(request.Query["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal))
                return string.Equals(request.Headers["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal);
            return true;
        }
    }
}
