using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UnityEngine
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Slider))]
    [AddComponentMenu("DataBinding/AutoTimerSliderBinder")]
    public sealed class AutoTimerSliderBinder : DataBinderBehaviour
    {
        public override List<Type> PropertyTypes { get; } = new()
        {
            typeof(bool),
        };

        [LabelText("是否正在运行")]
        [ReadOnly]
        public bool IsRunning;

        [LabelText("目标")]
        [ReadOnly]
        public Slider Target;

        private void Update()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (!this.IsRunning)
            {
                return;
            }

            this.Target.value = Mathf.Clamp(this.Target.value - Time.deltaTime, this.Target.minValue,
                this.Target.maxValue);

            if (Math.Abs(this.Target.value - this.Target.minValue) < float.Epsilon)
            {
                this.Target.value = this.Target.minValue;
                this.IsRunning = false;
            }
        }

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

            if (!this.IsRunning && this.GetValue(this.FirstDataBinder()))
            {
                this.IsRunning = true;
                this.Target.value = this.Target.maxValue;
                return;
            }

            if (this.IsRunning && !this.GetValue(this.FirstDataBinder()))
            {
                this.IsRunning = false;
            }
        }

        private bool GetValue(DataBinder binder)
        {
            if (binder.DataType == typeof(bool))
            {
                return this.dataSource.DataContext.GetValue<bool>(binder.Source);
            }

            return false;
        }

        public override void Initialize()
        {
            base.Initialize();
            this.Target ??= this.GetComponent<Slider>();
        }
    }
}