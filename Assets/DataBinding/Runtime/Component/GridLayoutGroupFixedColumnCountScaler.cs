namespace UnityEngine.UI
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(GridLayoutGroup))]
    public sealed class GridLayoutGroupFixedColumnCountScaler : MonoBehaviour
    {
        private RectTransform rectTransform;
        private GridLayoutGroup target;

        private void Awake()
        {
            this.rectTransform = this.GetComponent<RectTransform>();
            this.target = this.GetComponent<GridLayoutGroup>();

            this.Refresh();
        }

        public void Refresh()
        {
            var size = this.rectTransform.rect.size - Vector2.one;
            int count = (int)((size.x + this.target.spacing.x - this.target.padding.left - this.target.padding.right) /
                              (this.target.cellSize.x + this.target.spacing.x));
            if (count < 1)
            {
                return;
            }

            this.target.constraintCount = count;
            int offset =
                (int)((size.x + this.target.spacing.x - (this.target.cellSize.x + this.target.spacing.x) * count) *
                      0.5f);
            this.target.padding.left = offset;
            this.target.padding.right = offset;
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