using System;
using System.Collections.Generic;
using TMPro;

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
        public readonly List<Type> DataTypes = new()
        {
            typeof(int), typeof(float), typeof(long), typeof(bool), typeof(string),
            typeof(List<string>),
            typeof(List<TMP_Dropdown.OptionData>),
        };

        private readonly List<Type> dataContextTypes = new();

        /// <summary>
        /// 数据上下文类型集合
        /// </summary>
        public List<Type> DataContextTypes => this.dataContextTypes;

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