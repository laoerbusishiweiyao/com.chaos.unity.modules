using System;
using System.Collections.Generic;

namespace UnityEngine
{
    /// <summary>
    /// 数据上下文全局配置
    /// </summary>
    public sealed class DataContextOptions
    {
        public static readonly DataContextOptions Default = new();

        /// <summary>
        /// 数据上下文关联数据类型
        /// </summary>
        public readonly List<Type> DataTypes = new() { typeof(bool), typeof(int), typeof(string), };

        private readonly List<Type> dataContextTypes = new();

        /// <summary>
        /// 数据上下文类型集合
        /// </summary>
        public List<Type> DataContextTypes
        {
            get
            {
#if UNITY_EDITOR
                this.dataContextTypes.Clear();
                this.dataContextTypes.AddRange(UnityEditor.TypeCache.GetTypesDerivedFrom<DataContext>());
#else
                if (this.dataContextTypes.Count == 0)
                {
                    Debug.LogError("数据上下文类型集合为空");
                }
#endif

                return this.dataContextTypes;
            }
        }

        /// <summary>
        /// 加载所有数据上下文类型
        /// </summary>
        /// <param name="types"></param>
        public void LoadAllDataContextType(Dictionary<string, Type> types)
        {
            this.dataContextTypes.Clear();

            Type dataContextType = typeof(DataContext);
            foreach (Type type in types.Values)
            {
                if (!type.IsAbstract && type.IsSubclassOf(dataContextType))
                {
                    this.dataContextTypes.Add(type);
                }
            }
        }
    }
}