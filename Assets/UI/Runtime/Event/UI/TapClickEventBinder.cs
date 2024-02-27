using Sirenix.OdinInspector;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    public sealed class TapClickEventBinder : EventBinderBehaviour, IPointerClickHandler
    {
        [LabelText("点击间隔(ms)")]
        public int Interval = 500;

        [LabelText("触发次数")]
        [MinValue(1)]
        public int TapCountThreshold = 5;

        [LabelText("点击次数")]
        [ReadOnly]
        public int TapCount;

        private long time;

        public void OnPointerClick(PointerEventData eventData)
        {
            long now = (long)(Time.realtimeSinceStartup * 1000);
            if (this.time == 0)
            {
                this.time = now;
                this.TapCount = 0;
            }

            if (now - this.time < this.Interval)
            {
                this.TapCount += 1;
            }
            else
            {
                this.TapCount = 1;
            }

            this.time = now;

            if (this.TapCount < this.TapCountThreshold)
            {
                return;
            }
            
            this.OnEvent();

            this.time = 0;
            this.TapCount = 0;
        }
    }
}