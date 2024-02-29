using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;

namespace UnityEngine
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TextMeshProUGUI))]
    [AddComponentMenu("DataBinding/TextTMPBinder")]
    public sealed class TextTMPBinder : DataBinderBehaviour
    {
        public override List<Type> PropertyTypes { get; } = new()
        {
            typeof(int),
            typeof(string),
            typeof(float),
            typeof(long),
            typeof(bool),
        };
        
        [PropertySpace] [Delayed] [TextArea(2, 10)] [LabelText("字符串填充形式")]
        public string Format;

        [Delayed] [LabelText("数字精度")] public string Precision;

        [LabelText("目标")] [ReadOnly] public TextMeshProUGUI Target;

        private object[] parameters;

        public override void Refresh()
        {
            if (this.dataSource is null || this.dataSource.DataContext is null)
            {
                this.Target.text = string.Empty;
                return;
            }

            if (this.binders.Count == 0)
            {
                this.Target.text = string.Empty;
                return;
            }

            if (string.IsNullOrEmpty(this.Format))
            {
                this.Target.text = this.GetValue(this.FirstDataBinder());
            }
            else
            {
                parameters = new object[this.binders.Count];
                int index = 0;
                foreach (DataBinder binder in this.binders.Values)
                {
                    parameters[index++] = this.GetValue(binder);
                }

                try
                {
                    this.Target.text = string.Format(this.Format, parameters);
                }
                catch (Exception exception)
                {
                    Debug.LogError($"{this.name}字符串拼接 Format {this.Format} 出错请检查, {exception.Message}");
                }
            }
        }

        private string GetValue(DataBinder binder)
        {
            if (binder.DataType == typeof(string))
            {
                return this.dataSource.DataContext.GetValue<string>(binder.Source);
            }

            if (binder.DataType == typeof(int))
            {
                return this.dataSource.DataContext.GetValue<int>(binder.Source).ToString(this.Precision);
            }
            
            if (binder.DataType == typeof(long))
            {
                return this.dataSource.DataContext.GetValue<long>(binder.Source).ToString(this.Precision);
            }
            
            if (binder.DataType == typeof(float))
            {
                return this.dataSource.DataContext.GetValue<float>(binder.Source).ToString(this.Precision);
            }

            if (binder.DataType == typeof(bool))
            {
                return this.dataSource.DataContext.GetValue<bool>(binder.Source).ToString();
            }

            return string.Empty;
        }

        public override void Initialize()
        {
            base.Initialize();
            this.Target ??= this.GetComponent<TextMeshProUGUI>();
        }
    }
}