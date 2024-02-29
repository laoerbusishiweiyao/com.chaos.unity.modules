using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;

namespace UnityEngine
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TMP_Dropdown))]
    [AddComponentMenu("DataBinding/DropdownTMPOptionsBinder")]
    public sealed class DropdownTMPOptionsBinder : DataBinderBehaviour
    {
        public override List<Type> PropertyTypes { get; } = new()
        {
            typeof(List<TMP_Dropdown.OptionData>),
        };

        [LabelText("目标")]
        [ReadOnly]
        public TMP_Dropdown Target;

        public override void Refresh()
        {
            if (this.dataSource is null || this.dataSource.DataContext is null || this.Target is null)
            {
                this.Target.value = 0;
                return;
            }

            if (this.binders.Count == 0)
            {
                this.Target.value = 0;
                return;
            }

            this.Target.options.Clear();
            this.Target.AddOptions(this.GetValue(this.FirstDataBinder()));
            this.Target.RefreshShownValue();
        }

        private List<TMP_Dropdown.OptionData> GetValue(DataBinder binder)
        {
            if (binder.DataType == typeof(List<TMP_Dropdown.OptionData>))
            {
                return this.dataSource.DataContext.GetValue<List<TMP_Dropdown.OptionData>>(binder.Source);
            }

            return new List<TMP_Dropdown.OptionData>();
        }

        public override void Initialize()
        {
            base.Initialize();
            this.Target ??= this.GetComponent<TMP_Dropdown>();
        }
    }
}