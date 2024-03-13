using System;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    public sealed class PointerDownEventBinder : EventBinderBehaviour, IPointerDownHandler
    {
        [LabelText("拖拽时不响应点击")]
        public bool SkipWhenDrag = true;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (this.SkipWhenDrag && eventData.dragging)
            {
                return;
            }

            this.OnEvent();
        }
    }
}