using Newtonsoft.Json;
using Rongbo.Common.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Rongbo.Common.Utilities
{
    public class Serialization
    {

        /// <summary>
        /// Json序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <param name="indented"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static string JsonSerialize<T>(T val, bool indented, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(val, indented ? Formatting.Indented : Formatting.None, settings);
        }

        /// <summary>
        /// Json序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static string JsonSerialize<T>(T val, JsonSerializerSettings settings)
        {
            return JsonSerialize(val, false, settings);
        }

        /// <summary>
        /// Json序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <param name="indented"></param>
        /// <returns></returns>
        public static string JsonSerialize<T>(T val, bool indented)
        {
            return JsonSerialize(val, indented, GlobalSettings.JsonSettings);
        }

        /// <summary>
        /// Json序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string JsonSerialize<T>(T val)
        {
            return JsonSerialize(val, false);
        }

        /// <summary>
        /// Json反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static T JsonDeserialize<T>(string json, JsonSerializerSettings settings)
        {
            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        /// <summary>
        /// Json反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T JsonDeserialize<T>(string json)
        {
            return JsonDeserialize<T>(json, GlobalSettings.JsonSettings);
        }

        /// <summary>
        /// Json反序列化
        /// </summary>
        /// <param name="json"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static object JsonDeserialize(string json, JsonSerializerSettings settings)
        {
            return JsonConvert.DeserializeObject(json, settings);
        }

        /// <summary>
        ///  Json反序列化
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static object JsonDeserialize(string json)
        {
            return JsonConvert.DeserializeObject(json, GlobalSettings.JsonSettings);
        }

        /// <summary>
        /// 二进制序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        public static byte[] BinarySerialize<T>(T val)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, val);
            return stream.GetBuffer();
        }

        /// <summary>
        /// 二进制反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static T BinaryDeserialize<T>(byte[] buffer)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            return (T)formatter.Deserialize(new MemoryStream(buffer));
        }

        /// <summary>
        /// 字节集转结构
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static T BytesToStruct<T>(byte[] buffer) where T : struct
        {
            int len = Marshal.SizeOf<T>();
            IntPtr p = Marshal.AllocHGlobal(len);
            try
            {
                Marshal.Copy(buffer, 0, p, len);
                return Marshal.PtrToStructure<T>(p);
            }
            finally
            {
                Marshal.FreeHGlobal(p);
            }
        }

        /// <summary>
        /// 结构转字节集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        public static byte[] StructToBytes<T>(T val) where T : struct
        {
            int len = Marshal.SizeOf<T>();
            IntPtr p = Marshal.AllocHGlobal(len);
            try
            {
                Marshal.StructureToPtr(val, p, false);
                byte[] buffer = new byte[len];
                Marshal.Copy(p, buffer, 0, len);
                return buffer;
            }
            finally
            {
                Marshal.FreeHGlobal(p);
            }
        }
    }
}
