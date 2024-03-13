using System;

namespace UnityEngine.UI
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(GridLayoutGroup))]
    public sealed class GridLayoutGroupCellSizeScaler : MonoBehaviour
    {
        public int RowCount;
        public int ColumnCount;

        private RectTransform rectTransform;
        private GridLayoutGroup target;

        private void Awake()
        {
            this.rectTransform = this.GetComponent<RectTransform>();
            this.target = this.GetComponent<GridLayoutGroup>();

            this.Refresh();
        }

        private void OnEnable()
        {
            this.Refresh();
        }

        public void Refresh()
        {
            if (this.RowCount < 1 || this.ColumnCount < 1)
            {
                return;
            }

            var size = this.rectTransform.rect.size - Vector2.one;
            var spaceX = this.target.spacing.x * (this.ColumnCount - 1) + this.target.padding.horizontal;
            var spaceY = this.target.spacing.y * (this.RowCount - 1) + this.target.padding.vertical;
            this.target.cellSize = new((size.x - spaceX) / this.ColumnCount, (size.y - spaceY) / this.RowCount);
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            this.rectTransform = this.GetComponent<RectTransform>();
            this.target = this.GetComponent<GridLayoutGroup>();

            this.Refresh();
        }
#endif
    }
}