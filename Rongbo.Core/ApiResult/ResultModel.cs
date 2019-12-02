using System;
using System.Collections.Generic;
using System.Text;

namespace Rongbo.Core.ApiResult
{
    public class ResultModel
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }
    }

    public class ResultModel<T>: ResultModel
    {
        public T Data { get; set; }
    }
}
