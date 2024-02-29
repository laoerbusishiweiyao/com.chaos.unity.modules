using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;

namespace UnityEngine
{
    /// <summary>
    /// 数据上下文
    /// </summary>
    [HideReferenceObjectPicker]
    public abstract class DataContext
    {
        /// <summary>
        /// 访问器 - 数据上下文类型 属性值类型 属性路径 属性访问器
        /// </summary>
        public static readonly Dictionary<string, Dictionary<string, Dictionary<string, Func<DataContext, object>>>>
            Accessors = new();

        /// <summary>
        /// 修改器 - 数据上下文类型 属性值类型 属性路径 属性修改器
        /// </summary>
        public static readonly Dictionary<string, Dictionary<string, Dictionary<string, Action<DataContext, object>>>>
            Mutators = new();


        /// <summary>
        /// 基础数据路径 - 用于拼接属性路径
        /// </summary>
        [SerializeField]
        [ReadOnly]
        private string baseDataContextPath;

        /// <summary>
        /// 数据上下文类型全称 - 用于添加访问器
        /// </summary>
        private string contextTypeFullName;

        /// <summary>
        /// 数据上下文改变事件处理器
        /// </summary>
        private event EventHandler<DataContextChangedEventArgs> dataContextChanged;

        /// <summary>
        /// 当前数据上下文是否有监听器
        /// </summary>
        public bool HasListeners => this.dataContextChanged?.GetInvocationList().Length > 0;

        private DataContext()
        {
            this.contextTypeFullName = this.GetType().FullName;
        }

        public static void Build()
        {
            Accessors.Clear();
            Mutators.Clear();

            BuildAllTypeAccessor();
            BuildAllTypeMutator();
        }

        /// <summary>
        /// 动态构建所有数据上下文的访问器
        /// </summary>
        private static void BuildAllTypeAccessor()
        {
            foreach (Type type in DataContextOptions.Default.DataContextTypes)
            {
                Dictionary<string, Dictionary<string, Func<DataContext, object>>> valueAccessors = new();
                Accessors.Add(type.FullName, valueAccessors);
                foreach ((Type key, List<string> paths) in BuildAllTypeAccessor(type))
                {
                    Dictionary<string, Func<DataContext, object>> accessors = new();
                    valueAccessors.Add(key.FullName, accessors);

                    foreach (string path in paths)
                    {
                        var parameter = Expression.Parameter(typeof(DataContext));
                        var body = path.Split('.').Aggregate<string, Expression>(Expression.Convert(parameter, type),
                            Expression.Property);
                        var lambda =
                            Expression.Lambda<Func<DataContext, object>>(Expression.Convert(body, typeof(object)),
                                parameter);
                        var accessor = lambda.Compile();
                        accessors.Add(path, accessor);
                    }
                }
            }
        }

        /// <summary>
        /// 动态构建所有数据上下文的修改器
        /// </summary>
        private static void BuildAllTypeMutator()
        {
            foreach (Type type in DataContextOptions.Default.DataContextTypes)
            {
                Dictionary<string, Dictionary<string, Action<DataContext, object>>> valueMutators = new();
                Mutators.Add(type.FullName, valueMutators);
                foreach ((Type key, List<string> paths) in BuildAllTypeAccessor(type))
                {
                    Dictionary<string, Action<DataContext, object>> mutators = new();
                    valueMutators.Add(key.FullName, mutators);

                    foreach (string path in paths)
                    {
                        var parameter = Expression.Parameter(typeof(DataContext));
                        var value = Expression.Parameter(typeof(object));
                        var body = path.Split('.').Aggregate<string, Expression>(Expression.Convert(parameter, type),
                            Expression.Property);
                        var lambda =
                            Expression.Lambda<Action<DataContext, object>>(
                                Expression.Assign(body, Expression.Convert(value, body.Type)),
                                parameter, value);
                        var mutator = lambda.Compile();
                        mutators.Add(path, mutator);
                    }
                }
            }
        }

        /// <summary>
        /// 获取指定数据上下文类型的属性类型和同类型属性路径集合
        /// </summary>
        /// <param name="dataContextType">数据上下文类型</param>
        /// <returns></returns>
        private static Dictionary<Type, List<string>> BuildAllTypeAccessor(Type dataContextType)
        {
            Dictionary<Type, List<string>> propertyTypes = new();

            PropertyInfo[] propertyInfos =
                dataContextType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

            foreach (PropertyInfo info in propertyInfos)
            {
                if (DataContextOptions.Default.DataTypes.Contains(info.PropertyType))
                {
                    if (!propertyTypes.ContainsKey(info.PropertyType))
                    {
                        propertyTypes.Add(info.PropertyType, new List<string>());
                    }

                    propertyTypes[info.PropertyType].Add(info.Name);
                }
                else
                {
                    foreach ((Type type, List<string> paths) in BuildAllTypeAccessor(info.PropertyType))
                    {
                        if (!propertyTypes.ContainsKey(type))
                        {
                            propertyTypes.Add(type, new List<string>());
                        }

                        foreach (string path in paths)
                        {
                            propertyTypes[type].Add($"{info.Name}.{path}");
                        }
                    }
                }
            }

            return propertyTypes;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="baseDataContextPath">基础数据路径</param>
        protected DataContext(string baseDataContextPath = null) : this()
        {
            this.baseDataContextPath = baseDataContextPath;
        }

        /// <summary>
        /// 添加监听器
        /// </summary>
        /// <param name="listener"></param>
        public void AddListener(EventHandler<DataContextChangedEventArgs> listener)
        {
            this.dataContextChanged -= listener;
            this.dataContextChanged += listener;

            PropertyInfo[] propertyInfos = this.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

            foreach (PropertyInfo info in propertyInfos)
            {
                if (!info.PropertyType.IsSubclassOf(typeof(DataContext)) || info.PropertyType.IsAbstract)
                {
                    continue;
                }

                (info.GetValue(this) as DataContext)?.AddListener(this.OnDataContextChanged);
            }
        }

        /// <summary>
        /// 获取指定属性路径对应的值
        /// </summary>
        /// <param name="path">属性路径</param>
        /// <typeparam name="T">属性类型</typeparam>
        /// <returns></returns>
        public T GetValue<T>(string path)
        {
            if (this.contextTypeFullName is null)
            {
                this.contextTypeFullName = this.GetType().FullName;
            }

            if (this.contextTypeFullName is null)
            {
                Debug.LogError("数据上下文类型为空");
                return default;
            }

            if (!Accessors.ContainsKey(this.contextTypeFullName))
            {
                Debug.LogError($"类型 {this.contextTypeFullName} 不存在访问器");
                return default;
            }

            string valueTypeFullName = typeof(T).FullName;
            if (!Accessors[this.contextTypeFullName].ContainsKey(valueTypeFullName))
            {
                Debug.LogError($"类型 {this.contextTypeFullName} {valueTypeFullName} 不存在指定值类型访问器");
                return default;
            }

            if (!Accessors[this.contextTypeFullName][valueTypeFullName].ContainsKey(path))
            {
                Debug.LogError($"类型 {this.contextTypeFullName} {valueTypeFullName} 不存在指定路径 {path} 访问器");
                return default;
            }

            return (T)Accessors[this.contextTypeFullName][valueTypeFullName][path](this);
        }

        /// <summary>
        /// 设置指定属性路径对应的值
        /// </summary>
        /// <param name="path">属性路径</param>
        /// <typeparam name="T">属性类型</typeparam>
        /// <returns></returns>
        public void SetValue<T>(string path, T value)
        {
            if (this.contextTypeFullName is null)
            {
                this.contextTypeFullName = this.GetType().FullName;
            }

            if (this.contextTypeFullName is null)
            {
                Debug.LogError("数据上下文类型为空");
                return;
            }

            if (!Mutators.ContainsKey(this.contextTypeFullName))
            {
                Debug.LogError($"类型 {this.contextTypeFullName} 不存在修改器");
                return;
            }

            string valueTypeFullName = typeof(T).FullName;
            if (!Mutators[this.contextTypeFullName].ContainsKey(valueTypeFullName))
            {
                Debug.LogError($"类型 {this.contextTypeFullName} {valueTypeFullName} 不存在指定值类型修改器");
                return;
            }

            if (!Mutators[this.contextTypeFullName][valueTypeFullName].ContainsKey(path))
            {
                Debug.LogError($"类型 {this.contextTypeFullName} {valueTypeFullName} 不存在指定路径 {path} 修改器");
                return;
            }

            Mutators[this.contextTypeFullName][valueTypeFullName][path](this, value);
            this.dataContextChanged?.Invoke(this, new DataContextChangedEventArgs(path));
        }

        public void NotifyDataContextChanged(string propertyName)
        {
            string path = this.BuildPropertyPath(propertyName);
            this.dataContextChanged?.Invoke(this, new DataContextChangedEventArgs(path));
        }

        /// <summary>
        /// 数据上下文变化处理器 - 属性set时触发
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnDataContextChanged([CallerMemberName] string propertyName = null)
        {
            string path = this.BuildPropertyPath(propertyName);
            this.dataContextChanged?.Invoke(this, new DataContextChangedEventArgs(path));
        }

        /// <summary>
        /// 构建属性路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string BuildPropertyPath(string path)
        {
            return string.IsNullOrEmpty(this.baseDataContextPath)
                ? path
                : $"{this.baseDataContextPath}.{path}";
        }

        /// <summary>
        /// 设置属性路径
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            this.OnDataContextChanged(propertyName);
            return true;
        }

        /// <summary>
        /// 数据上下文变化处理器 - 向上传递
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void OnDataContextChanged(object sender, DataContextChangedEventArgs eventArgs)
        {
            string path = this.BuildPropertyPath(eventArgs.Path);
            this.dataContextChanged?.Invoke(this, new DataContextChangedEventArgs(path));
        }
    }
}