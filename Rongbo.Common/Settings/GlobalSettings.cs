using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rongbo.Common.Settings
{
    public static class GlobalSettings
    {
        static GlobalSettings()
        {
            //Json配置
            JsonSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateFormatString = "yyyy-MM-dd HH:mm:ss"
            };
        }

        /// <summary>
        /// Json序列化配置
        /// </summary>
        public static JsonSerializerSettings JsonSettings { get; }
    }
}
