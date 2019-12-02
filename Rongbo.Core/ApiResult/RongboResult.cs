using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Rongbo.Common.Enum;

namespace Rongbo.Core.ApiResult
{
    public class RongboResult : OkObjectResult
    {
        public RongboResult(StateCode code, dynamic data = null, string message = null) : base(null)
        {
            Value = new ResultModel<dynamic>
            {
                Code = (int)code,
                Data = data,
                Message = message
            };
        }

        public RongboResult(int code, dynamic data = null, string message = null) : base(null)
        {
            Value = new ResultModel<dynamic>
            {
                Code = code,
                Data = data,
                Message = message
            };
        }
    }
}
