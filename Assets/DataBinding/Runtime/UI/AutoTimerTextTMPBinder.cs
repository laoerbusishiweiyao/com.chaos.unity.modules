using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UnityEngine
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TextMeshProUGUI))]
    [AddComponentMenu("DataBinding/AutoTimerTextTMPBinder")]
    public sealed class AutoTimerTextTMPBinder : DataBinderBehaviour
    {
        public override List<Type> PropertyTypes { get; } = new()
        {
            typeof(bool),
        };

        [LabelText("持续时间")]
        public long MaxDuration;

        [PropertySpace]
        [Delayed]
        [TextArea(2, 10)]
        [LabelText("字符串填充形式")]
        public string Format;

        [Delayed]
        [LabelText("数字精度")]
        public string Precision;

        [LabelText("是否正在运行")]
        [ReadOnly]
        public bool IsRunning;

        [LabelText("目标")]
        [ReadOnly]
        public TextMeshProUGUI Target;

        private float duration;

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

            this.duration = Mathf.Clamp(this.duration - Time.deltaTime, 0, this.MaxDuration);
            this.SetText();

            if (this.duration < 0f)
            {
                this.duration = 0;
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

            if (!this.IsRunning && this.GetValue())
            {
                this.IsRunning = true;
                this.duration = this.MaxDuration;
                this.SetText();
                return;
            }

            if (this.IsRunning && !this.GetValue())
            {
                this.IsRunning = false;
            }
        }

        private void SetText()
        {
            if (string.IsNullOrEmpty(this.Format))
            {
                this.Target.text = this.duration.ToString(this.Precision);
            }
            else
            {
                try
                {
                    this.Target.text = string.Format(this.Format, this.duration.ToString(this.Precision));
                }
                catch (Exception exception)
                {
                    Debug.LogError($"{this.name}字符串拼接 Format {this.Format} 出错请检查, {exception.Message}");
                }
            }
        }

        private bool GetValue()
        {
            DataBinder binder = this.FirstDataBinder();
            if (binder.DataType == typeof(bool))
            {
                return this.dataSource.DataContext.GetValue<bool>(binder.Source);
            }

            return false;
        }

        public override void Initialize()
        {
            base.Initialize();
            this.Target ??= this.GetComponent<TextMeshProUGUI>();
        }
    }
}