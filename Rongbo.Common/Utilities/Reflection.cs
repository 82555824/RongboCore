using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.DotNet.PlatformAbstractions;
using System.Text;
using Microsoft.Extensions.DependencyModel;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Rongbo.Common.Utilities
{
    public static class Reflection
    {
        private static string[] _filter = { "System", "Microsoft", "netstandard", "dotnet", "Window", "mscorlib", "Nito" };

        private static HashSet<Assembly> _assemblies;

        public static IEnumerable<Assembly> Assemblies => _assemblies;

        static Reflection()
        {
            _assemblies = new HashSet<Assembly>();
            var identifier = RuntimeEnvironment.GetRuntimeIdentifier();
            var assemblyNames = DependencyContext.Default.GetRuntimeAssemblyNames(identifier);
            foreach (var assemblyName in assemblyNames)
            {
                if (!_filter.Any(o => assemblyName.FullName.StartsWith(o)))
                {
                    var assembly = GetAssembly(assemblyName);
                    _assemblies.Add(assembly);
                }
            }
        }

        /// <summary>
        /// 添加需要查询的程序集
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IEnumerable<Assembly> AddAssemblies(params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
                _assemblies.Add(assembly);
            return _assemblies;
        }

        /// <summary>
        /// 获取类型
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        public static Type GetType<T>()
        {
            return GetType(typeof(T));
        }

        /// <summary>
        /// 获取类型
        /// </summary>
        /// <param name="type">类型</param>
        public static Type GetType(Type type)
        {
            return Nullable.GetUnderlyingType(type) ?? type;
        }

        /// <summary>
        /// 获取类型描述，使用DescriptionAttribute设置描述
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        public static string GetDescription<T>()
        {
            return GetDescription(GetType<T>());
        }

        /// <summary>
        /// 获取类型成员描述，使用DescriptionAttribute设置描述
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="memberName">成员名称</param>
        public static string GetDescription<T>(string memberName)
        {
            return GetDescription(GetType<T>(), memberName);
        }

        /// <summary>
        /// 获取类型成员描述，使用DescriptionAttribute设置描述
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="memberName">成员名称</param>
        public static string GetDescription(Type type, string memberName)
        {
            if (type == null)
                return string.Empty;
            if (string.IsNullOrWhiteSpace(memberName))
                return string.Empty;
            return GetDescription(type.GetMember(memberName).FirstOrDefault());
        }

        /// <summary>
        /// 获取类型成员描述，使用DescriptionAttribute设置描述
        /// </summary>
        /// <param name="member">成员</param>
        public static string GetDescription(MemberInfo member)
        {
            if (member == null)
                return string.Empty;
            return member.GetCustomAttribute<DescriptionAttribute>() is DescriptionAttribute attribute ? attribute.Description : member.Name;
        }

        /// <summary>
        /// 获取显示名称，使用DisplayNameAttribute设置显示名称
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        public static string GetDisplayName<T>()
        {
            return GetDisplayName(GetType<T>());
        }

        /// <summary>
        /// 获取显示名称，使用DisplayAttribute或DisplayNameAttribute设置显示名称
        /// </summary>
        public static string GetDisplayName(MemberInfo member)
        {
            if (member == null)
                return string.Empty;
            if (member.GetCustomAttribute<DisplayAttribute>() is DisplayAttribute displayAttribute)
                return displayAttribute.Name;
            if (member.GetCustomAttribute<DisplayNameAttribute>() is DisplayNameAttribute displayNameAttribute)
                return displayNameAttribute.DisplayName;
            return string.Empty;
        }

        /// <summary>
        /// 获取显示名称或描述,使用DisplayNameAttribute设置显示名称,使用DescriptionAttribute设置描述
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        public static string GetDisplayNameOrDescription<T>()
        {
            return GetDisplayNameOrDescription(GetType<T>());
        }

        /// <summary>
        /// 获取属性显示名称或描述,使用DisplayAttribute或DisplayNameAttribute设置显示名称,使用DescriptionAttribute设置描述
        /// </summary>
        public static string GetDisplayNameOrDescription(MemberInfo member)
        {
            var result = GetDisplayName(member);
            return string.IsNullOrWhiteSpace(result) ? GetDescription(member) : result;
        }

        /// <summary>
        /// 获取实现了接口的所有类型
        /// </summary>
        /// <typeparam name="TInterface">接口类型</typeparam>
        /// <param name="assembly">在该程序集中查找</param>
        public static List<Type> GetTypesByInterface<TInterface>(Assembly assembly)
        {
            List<Type> ls = new List<Type>();
            try
            {
                var typeInterface = typeof(TInterface);
                ls = assembly.GetTypes()
                    .Where(t => typeInterface.IsAssignableFrom(t) && t != typeInterface && !t.IsAbstract).ToList();
            }
            catch (Exception)
            {
            }
            return ls;
        }

        /// <summary>
        /// 获取实现了接口的所有类型
        /// </summary>
        /// <typeparam name="TInterface">接口类型</typeparam>
        /// <param name="assemblies">在程序集数组中查找</param>
        /// <returns></returns>
        public static List<Type> GetTypesByInterface<TInterface>(IEnumerable<Assembly> assemblies)
        {
            List<Type> types = new List<Type>();
            foreach (var assembly in assemblies)
                types.AddRange(GetTypesByInterface<TInterface>(assembly));
            return types;
        }

        /// <summary>
        /// 从所有程序集获取实现了接口的所有类型
        /// </summary>
        /// <typeparam name="TInterface">接口类型</typeparam>
        /// <returns></returns>
        public static List<Type> GetTypesByInterface<TInterface>()
        {
            return GetTypesByInterface<TInterface>(_assemblies);
        }

        /// <summary>
        /// 获取程序集
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        public static Assembly GetAssembly(string assemblyName)
        {
            return Assembly.Load(new AssemblyName(assemblyName));
        }

        /// <summary>
        /// 获取程序集
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public static Assembly GetAssembly(AssemblyName assemblyName)
        {
            try
            {
                return Assembly.Load(assemblyName);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 从目录中获取所有程序集
        /// </summary>
        /// <param name="directoryPath">目录绝对路径</param>
        public static List<Assembly> GetAssemblies(string directoryPath)
        {
            return Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories).ToList()
                .Where(t => t.EndsWith(".exe") || t.EndsWith(".dll"))
                .Select(path => Assembly.Load(new AssemblyName(path))).ToList();
        }

        /// <summary>
        /// 获取顶级基类
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        public static Type GetTopBaseType<T>()
        {
            return GetTopBaseType(typeof(T));
        }

        /// <summary>
        /// 获取顶级基类
        /// </summary>
        /// <param name="type">类型</param>
        public static Type GetTopBaseType(Type type)
        {
            if (type == null)
                return null;
            if (type.IsInterface)
                return type;
            if (type.BaseType == typeof(object))
                return type;
            return GetTopBaseType(type.BaseType);
        }
    }
}
