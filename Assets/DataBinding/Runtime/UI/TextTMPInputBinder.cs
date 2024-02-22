using System;
using Sirenix.OdinInspector;
using TMPro;

namespace UnityEngine
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TMP_InputField))]
    [AddComponentMenu("DataBinding/TextTMPBinder")]
    public sealed class TextTMPInputBinder : DataBinderBehaviour
    {
        [Delayed] [LabelText("数字精度")] public string Precision;

        [LabelText("目标")] [ReadOnly] public TMP_InputField Target;

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
            if (this.dataSource is null || this.dataSource.DataContext is null)
            {
                return;
            }

            if (this.binders.Count == 0)
            {
                return;
            }

            this.dataSource.DataContext.SetValue(this.FirstDataBinder().Source, value);
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