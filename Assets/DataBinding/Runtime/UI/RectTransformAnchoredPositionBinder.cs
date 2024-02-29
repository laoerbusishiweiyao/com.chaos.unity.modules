using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace UnityEngine
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    [AddComponentMenu("DataBinding/RectTransformAnchoredPositionBinder")]
    public sealed class RectTransformAnchoredPositionBinder : DataBinderBehaviour
    {
        public override List<Type> PropertyTypes { get; } = new()
        {
            typeof(List<float>),
            typeof(List<long>),
            typeof(List<int>),
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

            this.SetAnchoredPosition();
        }

        private void SetAnchoredPosition()
        {
            var binder = this.FirstDataBinder();
            if (binder.DataType == typeof(List<float>))
            {
                var values = this.dataSource.DataContext.GetValue<List<float>>(binder.Source);
                if (values.Count == 1)
                {
                    this.Target.anchoredPosition = new Vector2(values[0], values[0]);
                }
                else if (values.Count > 1)
                {
                    this.Target.anchoredPosition = new Vector2(values[0], values[1]);
                }
            }

            if (binder.DataType == typeof(List<int>))
            {
                var values = this.dataSource.DataContext.GetValue<List<int>>(binder.Source);
                if (values.Count == 1)
                {
                    this.Target.anchoredPosition = new Vector2(values[0], values[0]);
                }
                else if (values.Count > 1)
                {
                    this.Target.anchoredPosition = new Vector2(values[0], values[1]);
                }
            }

            if (binder.DataType == typeof(List<long>))
            {
                var values = this.dataSource.DataContext.GetValue<List<long>>(binder.Source);
                if (values.Count == 1)
                {
                    this.Target.anchoredPosition = new Vector2(values[0], values[0]);
                }
                else if (values.Count > 1)
                {
                    this.Target.anchoredPosition = new Vector2(values[0], values[1]);
                }
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            this.Target ??= this.GetComponent<RectTransform>();
        }
    }
}