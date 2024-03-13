using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;

namespace UnityEngine
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    [AddComponentMenu("DataBinding/RectTransformWidthBinder")]
    public sealed class RectTransformWidthBinder : DataBinderBehaviour
    {
        public override List<Type> PropertyTypes { get; } = new()
        {
            typeof(int),
            typeof(float),
            typeof(long),
        };

        [LabelText("目标")]
        [ReadOnly]
        public RectTransform Target;

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

            this.Target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.GetValue(this.FirstDataBinder()));
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
            this.Target ??= this.GetComponent<RectTransform>();
        }
    }
}