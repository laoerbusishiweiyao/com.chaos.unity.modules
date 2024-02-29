using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace UnityEngine
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Graphic))]
    [AddComponentMenu("DataBinding/GraphicEnableCompareBinder")]
    public sealed class GraphicEnableCompareBinder : DataBinderBehaviour
    {
        public override List<Type> PropertyTypes { get; } = new()
        {
            typeof(bool),
            typeof(long),
            typeof(int),
            typeof(float),
        };
        
        public UICompareMode CompareMode;

        [LabelText("目标值")]
        public string Value;

        [LabelText("目标")]
        [ReadOnly]
        public Graphic Target;

        public override void Refresh()
        {
            if (this.dataSource is null || this.dataSource.DataContext is null || this.Target is null)
            {
                this.Target.enabled = true;
                return;
            }

            if (this.binders.Count == 0)
            {
                this.Target.enabled = true;
                return;
            }

            if (string.IsNullOrEmpty(this.Value))
            {
                this.Target.enabled = true;
                return;
            }

            this.Target.enabled = this.GetValue(this.FirstDataBinder());
        }

        private bool GetValue(DataBinder binder)
        {
            if (binder.DataType == typeof(int))
            {
                int current = this.dataSource.DataContext.GetValue<int>(binder.Source);
                int value = int.Parse(this.Value);
                return this.CompareMode switch
                {
                    UICompareMode.Less => current < value,
                    UICompareMode.LessEqual => current <= value,
                    UICompareMode.Equal => current == value,
                    UICompareMode.Great => current > value,
                    UICompareMode.GreatEqual => current >= value,
                    UICompareMode.NotEqual => current != value,
                    _ => true
                };
            }

            if (binder.DataType == typeof(long))
            {
                long current = this.dataSource.DataContext.GetValue<long>(binder.Source);
                long value = long.Parse(this.Value);
                return this.CompareMode switch
                {
                    UICompareMode.Less => current < value,
                    UICompareMode.LessEqual => current <= value,
                    UICompareMode.Equal => current == value,
                    UICompareMode.Great => current > value,
                    UICompareMode.GreatEqual => current >= value,
                    UICompareMode.NotEqual => current != value,
                    _ => true
                };
            }

            if (binder.DataType == typeof(float))
            {
                float current = this.dataSource.DataContext.GetValue<float>(binder.Source);
                float value = float.Parse(this.Value);
                return this.CompareMode switch
                {
                    UICompareMode.Less => current < value,
                    UICompareMode.LessEqual => current <= value,
                    UICompareMode.Equal => current == value,
                    UICompareMode.Great => current > value,
                    UICompareMode.GreatEqual => current >= value,
                    UICompareMode.NotEqual => current != value,
                    _ => true
                };
            }

            if (binder.DataType == typeof(bool))
            {
                bool current = this.dataSource.DataContext.GetValue<bool>(binder.Source);
                bool value = bool.Parse(this.Value);
                return this.CompareMode switch
                {
                    UICompareMode.Equal => current == value,
                    UICompareMode.NotEqual => current != value,
                    _ => true
                };
            }

            return true;
        }

        public override void Initialize()
        {
            base.Initialize();
            this.Target ??= this.GetComponent<Graphic>();
        }
    }
}