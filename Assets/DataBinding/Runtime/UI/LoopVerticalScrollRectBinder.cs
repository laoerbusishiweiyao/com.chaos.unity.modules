using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace UnityEngine
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(LoopVerticalScrollRect))]
    [AddComponentMenu("DataBinding/LoopVerticalScrollRectBinder")]
    public sealed class LoopVerticalScrollRectBinder : DataBinderBehaviour, LoopScrollDataSource, LoopScrollPrefabSource
    {
        public static event Action<Transform, int, LoopScrollRectItem> OnLoopVerticalScrollRectProvideData;

        public override List<Type> PropertyTypes { get; } = new()
        {
            typeof(List<LoopScrollRectItem>),
        };

        [LabelText("模板")]
        public GameObject Template;

        private List<LoopScrollRectItem> items = new();

        [LabelText("目标")]
        [ReadOnly]
        public LoopVerticalScrollRect Target;

        private Stack<GameObject> pool = new();

        public override void Refresh()
        {
            if (this.dataSource is null || this.dataSource.DataContext is null)
            {
                return;
            }

            if (this.binders.Count == 0)
            {
                return;
            }

            this.items.Clear();
            this.items.AddRange(this.GetValue());
            this.Target.totalCount = this.items.Count;
            this.Target.ClearCells();
            if (this.items.Count > 0)
            {
                this.Target.RefillCells();
            }
        }

        private List<LoopScrollRectItem> GetValue()
        {
            var binder = this.FirstDataBinder();
            if (binder.DataType == typeof(List<LoopScrollRectItem>))
            {
                return this.dataSource.DataContext.GetValue<List<LoopScrollRectItem>>(binder.Source);
            }

            return new List<LoopScrollRectItem>();
        }

        public override void Initialize()
        {
            base.Initialize();
            this.Target ??= this.GetComponent<LoopVerticalScrollRect>();

            this.Target.prefabSource = this;
            this.Target.dataSource = this;
            this.Target.totalCount = this.items.Count;
            this.Target.ClearCells();
            if (this.items.Count > 0)
            {
                this.Target.RefillCells();
            }
        }

        public void ProvideData(Transform sd, int idx)
        {
            OnLoopVerticalScrollRectProvideData?.Invoke(sd, idx, this.items[idx]);
        }

        public GameObject GetObject(int index)
        {
            if (pool.Count == 0)
            {
                return Instantiate(this.Template);
            }

            GameObject candidate = pool.Pop();
            candidate.SetActive(true);
            return candidate;
        }

        public void ReturnObject(Transform trans)
        {
            trans.gameObject.SetActive(false);
            trans.SetParent(transform, false);
            pool.Push(trans.gameObject);
        }
    }

    [HideReferenceObjectPicker]
    [Serializable]
    public sealed class LoopScrollRectItem
    {
        public int Index;

        public object Data;
    }
}