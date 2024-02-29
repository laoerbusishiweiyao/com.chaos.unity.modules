using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;

namespace UnityEngine
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TMP_InputField))]
    [AddComponentMenu("DataBinding/TextTMPInputBinder")]
    public sealed class TextTMPInputBinder : DataBinderBehaviour
    {
        public override List<Type> PropertyTypes { get; } = new()
        {
            typeof(int),
            typeof(string),
            typeof(float),
            typeof(long),
            typeof(bool),
        };
        
        [Delayed]
        [LabelText("数字精度")]
        public string Precision;

        [LabelText("目标")]
        [ReadOnly]
        public TMP_InputField Target;

        public override void Refresh()
        {
            if (this.dataSource is null || this.dataSource.DataContext is null || this.Target is null)
            {
                this.Target.text = string.Empty;
                return;
            }

            if (this.binders.Count == 0)
            {
                this.Target.text = string.Empty;
                return;
            }

            this.Target.text = this.GetValue(this.FirstDataBinder());
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

            if (binder.DataType == typeof(float))
            {
                return this.dataSource.DataContext.GetValue<float>(binder.Source).ToString(this.Precision);
            }

            if (binder.DataType == typeof(long))
            {
                return this.dataSource.DataContext.GetValue<long>(binder.Source).ToString(this.Precision);
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
            this.Target ??= this.GetComponent<TMP_InputField>();

            this.Target.onEndEdit.RemoveListener(UpdateSource);
            this.Target.onEndEdit.AddListener(UpdateSource);
        }

        private void UpdateSource(string value)
        {
            if (this.dataSource is null || this.dataSource.DataContext is null || this.Target is null)
            {
                return;
            }

            if (this.binders.Count == 0)
            {
                return;
            }

            DataBinder binder = this.FirstDataBinder();
            if (binder.DataType == typeof(string))
            {
                this.dataSource.DataContext.SetValue(binder.Source, value);
            }

            if (binder.DataType == typeof(int))
            {
                this.dataSource.DataContext.SetValue(binder.Source, int.Parse(value));
            }

            if (binder.DataType == typeof(float))
            {
                this.dataSource.DataContext.SetValue(binder.Source, float.Parse(value));
            }

            if (binder.DataType == typeof(long))
            {
                this.dataSource.DataContext.SetValue(binder.Source, long.Parse(value));
            }

            if (binder.DataType == typeof(bool))
            {
                this.dataSource.DataContext.SetValue(binder.Source, bool.Parse(value));
            }
        }

        [OnInspectorGUI]
        private void OnInspectorGUI()
        {
            if (this.Target is null)
            {
                return;
            }

            this.UpdateSource(this.Target.text);
        }
    }
}