using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    [DisallowMultipleComponent]
    public sealed class DropHandler : UIBehaviour, IDropHandler
    {
        public static event Action<DropHandler, PointerEventData> OnDropHandler;

        public void OnDrop(PointerEventData eventData)
        {
            OnDropHandler?.Invoke(this, eventData);
        }
    }
}