#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace UnityEditor
{
    /// <summary>
    /// 数据上下文缓存 - 属性路径、属性类型
    /// </summary>
    public static class DataContextCache
    {
        /// <summary>
        /// 指定数据上下文类型对应的属性路径集合
        /// </summary>
        private static readonly Dictionary<Type, List<string>> pathCaches = new();

        /// <summary>
        /// 指定数据上下文类型对应的属性类型映射关系字典
        /// </summary>
        private static readonly Dictionary<Type, Dictionary<string, Type>> typeCaches = new();

        /// <summary>
        /// 过滤获取指定数据上下文类型的属性路径
        /// </summary>
        /// <param name="dataContextType">数据上下文类型</param>
        /// <returns></returns>
        public static List<string> FilterPropertyPath(Type dataContextType, List<Type> propertyTypes)
        {
            if (!pathCaches.ContainsKey(dataContextType))
            {
                Cache();
            }

            if (!pathCaches.ContainsKey(dataContextType))
            {
                Debug.LogError($"类型 {dataContextType} 未缓存属性路径");
                return default;
            }

            if (propertyTypes.Count == 0)
            {
                return pathCaches[dataContextType];
            }

            List<string> paths = new();
            foreach (string path in pathCaches[dataContextType])
            {
                if (propertyTypes.Contains(PropertyType(dataContextType, path)))
                {
                    paths.Add(path);
                }
            }

            return paths;
        }

        /// <summary>
        /// 获取指定数据上下文类型中指定属性路径对应的类型
        /// </summary>
        /// <param name="dataContextType">数据上下文类型</param>
        /// <param name="path">属性路径</param>
        /// <returns></returns>
        public static Type PropertyType(Type dataContextType, string path)
        {
            if (!typeCaches.ContainsKey(dataContextType))
            {
                Debug.LogError($"类型 {dataContextType} 未缓存属性类型");
                return default;
            }

            if (!typeCaches[dataContextType].ContainsKey(path))
            {
                Debug.LogError($"类型 {dataContextType} 路径 {path} 未缓存属性类型");
                return default;
            }

            return typeCaches[dataContextType][path];
        }

        public static void Cache()
        {
            foreach (Type type in DataContextOptions.Default.DataContextTypes)
            {
                Cache(type);
            }
        }

        /// <summary>
        /// 缓存指定数据上下文类型的属性路径和属性类型映射
        /// </summary>
        /// <param name="dataContextType">数据上下文类型</param>
        private static void Cache(Type dataContextType)
        {
            if (dataContextType is null)
            {
                return;
            }

            CachePropertyPath(dataContextType);
            CachePropertyType(dataContextType);
        }

        /// <summary>
        /// 缓存指定数据上下文类型的属性路径
        /// </summary>
        /// <param name="dataContextType">数据上下文类型</param>
        private static void CachePropertyPath(Type dataContextType)
        {
            pathCaches.Remove(dataContextType);
            pathCaches.Add(dataContextType, BuildAllPropertyPath(dataContextType));
        }

        /// <summary>
        /// 构建指定数据上下文的属性路径
        /// </summary>
        /// <param name="dataContextType">数据上下文类型</param>
        /// <returns></returns>
        private static List<string> BuildAllPropertyPath(Type dataContextType)
        {
            var paths = new List<string>();

            PropertyInfo[] propertyInfos =
                dataContextType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

            foreach (PropertyInfo info in propertyInfos)
            {
                if (DataContextOptions.Default.DataTypes.Contains(info.PropertyType))
                {
                    paths.Add(info.Name);
                }
                else
                {
                    foreach (string path in BuildAllPropertyPath(info.PropertyType))
                    {
                        paths.Add($"{info.Name}.{path}");
                    }
                }
            }

            return paths;
        }

        /// <summary>
        /// 缓存指定数据上下文类型的属性类型映射
        /// </summary>
        /// <param name="dataContextType">数据上下文类型</param>
        private static void CachePropertyType(Type dataContextType)
        {
            typeCaches.Remove(dataContextType);
            typeCaches.Add(dataContextType, BuildAllPropertyType(dataContextType));
        }

        /// <summary>
        /// 缓存指定数据上下文类型的属性类型映射
        /// </summary>
        /// <param name="dataContextType">数据上下文类型</param>
        /// <returns></returns>
        private static Dictionary<string, Type> BuildAllPropertyType(Type dataContextType)
        {
            Dictionary<string, Type> propertyTypes = new();

            PropertyInfo[] propertyInfos =
                dataContextType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

            foreach (PropertyInfo info in propertyInfos)
            {
                if (DataContextOptions.Default.DataTypes.Contains(info.PropertyType))
                {
                    propertyTypes.Add(info.Name, info.PropertyType);
                }
                else
                {
                    foreach ((string path, Type value) in BuildAllPropertyType(info.PropertyType))
                    {
                        propertyTypes.Add($"{info.Name}.{path}", value);
                    }
                }
            }

            return propertyTypes;
        }
    }
}
#endif