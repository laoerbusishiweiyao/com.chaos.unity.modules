using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
    [DisallowMultipleComponent]
    public sealed class DragHandler : UIBehaviour, IInitializePotentialDragHandler, IBeginDragHandler, IDragHandler,
        IEndDragHandler
    {
        [LabelText("拖拽结束自动复位")]
        public bool ResetOnEndDrag;

        [FormerlySerializedAs("PassOnEndDrag")]
        [LabelText("拖拽结束透传Drop")]
        public bool PassDrop;

        private RectTransform rectTransform;
        private Vector3 beginDragPosition;
        private Vector3 beginWorldPoint;
        private Vector3 worldPoint;

        protected override void Awake()
        {
            base.Awake();
            this.rectTransform = this.GetComponent<RectTransform>();
        }

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            this.beginDragPosition = this.rectTransform.position;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(this.rectTransform, eventData.position,
                eventData.pressEventCamera, out this.beginWorldPoint);
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(this.rectTransform, eventData.position,
                eventData.pressEventCamera, out this.worldPoint);
            var offset = new Vector3(this.worldPoint.x - this.beginWorldPoint.x,
                this.worldPoint.y - this.beginWorldPoint.y, 0);
            this.rectTransform.position = this.beginDragPosition + offset;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (this.PassDrop)
            {
                this.PassEvent(eventData, ExecuteEvents.dropHandler);
            }

            if (this.ResetOnEndDrag)
            {
                this.rectTransform.position = this.beginDragPosition;
            }
        }

        private void PassEvent<T>(PointerEventData eventData, ExecuteEvents.EventFunction<T> function)
            where T : IEventSystemHandler
        {
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            GameObject current = eventData.pointerDrag;
            foreach (var result in results)
            {
                if (current != result.gameObject)
                {
                    eventData.pointerPressRaycast = result;
                    ExecuteEvents.Execute(result.gameObject, eventData, function);
                }
            }
        }
    }
}