using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using NLog;
using NLog.Config;
using NLog.LayoutRenderers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Rongbo.Common.NLog
{
    public class WebInfoLayoutRenderer : LayoutRenderer
    {
        internal static IHttpContextAccessor httpContextAccessor;

        public WebInfoLayoutRenderer() { }

        /// <summary>
        /// 记录 Cookies 默认关闭
        /// </summary>
        public bool Cookie { get; set; } = false;

        private LogLevel _minLevel = LogLevel.Warn;

        /// <summary>
        /// 大于等于该级别的日志 也会带上信息 默认 Warn
        /// </summary>
        [DefaultParameter]
        public string MinLevel
        {
            get
            {
                return _minLevel.Name;
            }
            set
            {
                _minLevel = LogLevel.FromString(value);
            }
        }

        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            var httpContext = httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                var requestStarting = logEvent.CallerMemberName == "ExecutingAction";
                if (logEvent.Level >= _minLevel || requestStarting)
                {
                    var request = httpContext.Request;
                    if (requestStarting)
                    {
                        builder.AppendLine($" {httpContext.Connection.RemoteIpAddress}");
                    }
                    else
                    {
                        builder.AppendLine();
                        builder.AppendLine($"   {request.Protocol} {request.Method} {request.Scheme}://{request.Host}{request.Path}{request.QueryString}   {httpContext.Connection.RemoteIpAddress}");
                    }
                    if (request.Headers.ContainsKey("User-Agent"))
                        builder.AppendLine($"   User-Agent: {request.Headers["User-Agent"]}");
                    if (request.ContentType != null)
                        builder.AppendLine($"   Content-Type: {request.ContentType}");
                    if (Cookie && request.Cookies.Count > 0)
                        builder.AppendLine($"   Cookies: {string.Join(" | ", request.Cookies)}");
                    if (request.Headers.ContainsKey("Authorization"))
                        builder.AppendLine($"   Authorization: {request.Headers["Authorization"]}");
                    if (httpContext.User.Identity.IsAuthenticated)
                        builder.AppendLine($"   Identity: {string.Join(" | ", httpContext.User.Claims)}");
                    //if (request.Method != "GET")
                    //    builder.AppendLine($"   Body: {ReadBodyString(request)}");
                }
            }
        }

        private bool IsMultipartContentType(string contentType)
        {
            return !string.IsNullOrEmpty(contentType)
                   && contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private string ReadBodyString(HttpRequest request)
        {
            string result = string.Empty;
            if (!IsMultipartContentType(request.ContentType) && request.Body != null)
            {
                if (!request.Body.CanSeek)
                    request.EnableRewind();

                using (var memoryStream = new MemoryStream())
                {
                    using (var reader = new StreamReader(memoryStream, Encoding.UTF8))
                    {
                        request.Body.Position = 0;
                        request.Body.CopyTo(memoryStream);
                        memoryStream.Seek(0, SeekOrigin.Begin);
                        result = reader.ReadToEnd();
                    }
                }

                request.Body.Position = 0;
            }

            return result;
        }
    }
}
