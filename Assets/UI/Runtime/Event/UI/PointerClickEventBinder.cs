using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    public sealed class PointerClickEventBinder : EventBinderBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            this.OnEvent();
        }
    }
}