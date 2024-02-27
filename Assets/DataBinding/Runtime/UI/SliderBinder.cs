using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;

namespace UnityEngine
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Slider))]
    [AddComponentMenu("DataBinding/SliderBinder")]
    public sealed class SliderBinder : DataBinderBehaviour
    {
        [LabelText("目标")]
        [ReadOnly]
        public Slider Target;

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

            this.Target.value = this.GetValue(this.FirstDataBinder());
        }

        private float GetValue(DataBinder binder)
        {
            if (binder.DataType == typeof(int))
            {
                return this.dataSource.DataContext.GetValue<int>(binder.Source);
            }
            
            if (binder.DataType == typeof(float))
            {
                return this.dataSource.DataContext.GetValue<float>(binder.Source);
            }
            
            if (binder.DataType == typeof(long))
            {
                return this.dataSource.DataContext.GetValue<long>(binder.Source);
            }

            return 0;
        }

        public override void Initialize()
        {
            base.Initialize();
            this.Target ??= this.GetComponent<Slider>();
        }
    }
}