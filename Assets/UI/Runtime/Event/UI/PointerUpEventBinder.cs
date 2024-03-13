using Sirenix.OdinInspector;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    public sealed class PointerUpEventBinder : EventBinderBehaviour, IPointerUpHandler
    {
        [LabelText("拖拽时不响应点击")]
        public bool SkipWhenDrag = true;

        [PropertySpace]
        [InfoBox("指针移出时是否在新的指针移入并且监听指针抬起事件的物体上触发")]
        [LabelText("转移事件触发")]
        public bool TransferWhenExit;

        public void OnPointerUp(PointerEventData eventData)
        {
            if (this.SkipWhenDrag && eventData.dragging)
            {
                return;
            }
            
            if (this.TransferWhenExit && eventData.pointerEnter != this.gameObject &&
                eventData.pointerEnter.GetComponent<PointerUpEventBinder>() is { } pointerUpEventBinder)
            {
                pointerUpEventBinder.OnPointerUp(eventData);
                return;
            }
            
            this.OnEvent();
        }
    }
}