using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace UnityEngine
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Image))]
    [AddComponentMenu("DataBinding/ImageFillAmountBinder")]
    public sealed class ImageFillAmountBinder : DataBinderBehaviour
    {
        public override List<Type> PropertyTypes { get; } = new()
        {
            typeof(float),
        };
        
        [LabelText("目标")]
        [ReadOnly]
        public Image Target;

        public override void Refresh()
        {
            if (this.dataSource is null || this.dataSource.DataContext is null || this.Target is null)
            {
                return;
            }

            if (this.binders.Count == 0)
            {
                return;
            }

            this.Target.fillAmount = this.GetValue(this.FirstDataBinder());
        }

        private float GetValue(DataBinder binder)
        {
            if (binder.DataType == typeof(float))
            {
                return this.dataSource.DataContext.GetValue<float>(binder.Source);
            }

            return 0;
        }

        public override void Initialize()
        {
            base.Initialize();
            this.Target ??= this.GetComponent<Image>();
        }
    }
}