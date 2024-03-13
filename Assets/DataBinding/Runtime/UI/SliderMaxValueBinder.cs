using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;

namespace UnityEngine
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Slider))]
    [AddComponentMenu("DataBinding/SliderMaxValueBinder")]
    public sealed class SliderMaxValueBinder : DataBinderBehaviour
    {
        public override List<Type> PropertyTypes { get; } = new()
        {
            typeof(int),
            typeof(float),
            typeof(long),
        };

        [LabelText("目标")]
        [ReadOnly]
        public Slider Target;

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

            this.Target.maxValue = Math.Max(this.GetValue(this.FirstDataBinder()), this.Target.minValue);
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