using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;

namespace UnityEngine
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TMP_Dropdown))]
    [AddComponentMenu("DataBinding/DropdownTMPBinder")]
    public sealed class DropdownTMPBinder : DataBinderBehaviour
    {
        [LabelText("目标")]
        [ReadOnly]
        public TMP_Dropdown Target;

        public override void Refresh()
        {
            if (this.dataSource is null || this.dataSource.DataContext is null)
            {
                this.Target.value = 0;
                return;
            }

            if (this.binders.Count == 0)
            {
                this.Target.value = 0;
                return;
            }

            this.Target.onValueChanged.RemoveListener(this.OnValueChanged);
            
            this.Target.value = this.GetValue(this.FirstDataBinder());
            
            this.Target.onValueChanged.AddListener(this.OnValueChanged);
        }

        private int GetValue(DataBinder binder)
        {
            if (binder.DataType == typeof(int))
            {
                return this.dataSource.DataContext.GetValue<int>(binder.Source);
            }

            return 0;
        }

        public override void Initialize()
        {
            base.Initialize();
            this.Target ??= this.GetComponent<TMP_Dropdown>();

            this.Target.onValueChanged.RemoveListener(this.OnValueChanged);
            this.Target.onValueChanged.AddListener(this.OnValueChanged);
        }

        private void OnValueChanged(int value)
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

            this.OnValueChanged(this.Target.value);
        }
    }
}