using UnityEngine.UI;

namespace UnityEngine
{
    [RequireComponent(typeof(CanvasRenderer))]
    public sealed class UITrigger : Graphic, ICanvasRaycastFilter
    {
        public override bool raycastTarget
        {
            get => true;
            set { }
        }

        public override Texture mainTexture => null;

        public override Material materialForRendering => null;

        public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            return true;
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
        }

        public override void SetAllDirty()
        {
        }

        public override void SetLayoutDirty()
        {
        }

        public override void SetVerticesDirty()
        {
        }

        public override void SetMaterialDirty()
        {
        }
    }
}