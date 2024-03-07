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

        [LabelText("拖拽结束透传Drop")]
        public bool PassDrop;

        [LabelText("开始拖拽时改变父节点")]
        public bool ChangeParentOnBegin;

        [ShowIf(nameof(ChangeParentOnBegin))]
        [LabelText("开始拖拽时目标父节点")]
        public RectTransform ParentOnBegin;

        [LabelText("结束拖拽时复位父节点")]
        public bool ResetParentOnEnd;

        [ShowIf(nameof(ResetParentOnEnd))]
        [LabelText("初始父节点")]
        public RectTransform InitialParent;

        private RectTransform rectTransform;
        private Vector3 beginDragPosition;
        private Vector3 beginWorldPoint;
        private Vector3 worldPoint;

        protected override void Awake()
        {
            base.Awake();
            this.rectTransform = this.GetComponent<RectTransform>();
            if (this.ResetOnEndDrag)
            {
                this.InitialParent = this.rectTransform.parent as RectTransform;
            }
        }

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            this.beginDragPosition = this.rectTransform.position;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(this.rectTransform, eventData.position,
                eventData.pressEventCamera, out this.beginWorldPoint);

            if (this.ChangeParentOnBegin && this.ParentOnBegin)
            {
                this.rectTransform.SetParent(this.ParentOnBegin);
            }
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
            if (this.ResetOnEndDrag && this.InitialParent)
            {
                this.rectTransform.SetParent(this.InitialParent);
            }

            if (this.ResetOnEndDrag)
            {
                this.rectTransform.position = this.beginDragPosition;
            }

            if (this.PassDrop)
            {
                this.PassEvent(eventData, ExecuteEvents.dropHandler);
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
                    ExecuteEvents.Execute(result.gameObject, eventData, function);
                }
            }
        }
    }
}