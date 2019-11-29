using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Rongbo.Common.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// 获取当前登录用户ID
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static long GetUserId(this ClaimsPrincipal principal)
        {
            if (principal.Identity.IsAuthenticated)
            {
                var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
                if (claim != null)
                    return Convert.ToInt64(claim.Value);
            }
            return 0;
        }
    }
}
