using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rongbo.Core.DependencyInjection
{
    public class ContextAccessor
    {
        public static IHttpContextAccessor _httpContextAccessor;

        public static HttpContext GetHttpContext()
        {
            return _httpContextAccessor.HttpContext;
        }
    }
}
