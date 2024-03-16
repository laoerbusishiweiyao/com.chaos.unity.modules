using System;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    [DisallowMultipleComponent]
    public sealed class DropHandler : UIBehaviour, IDropHandler
    {
        public static event Action<DropHandler, PointerEventData> OnDropHandler;

        [LabelText("是否为异步任务")]
        public bool IsTask;

        [HideInInspector]
        public bool IsTasking;

        public void OnDrop(PointerEventData eventData)
        {
            if (this.IsTask && this.IsTasking)
            {
                return;
            }

            OnDropHandler?.Invoke(this, eventData);
        }
    }
}