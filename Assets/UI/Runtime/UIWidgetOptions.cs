using Sirenix.OdinInspector;

namespace UnityEngine
{
    [AddComponentMenu("")]
    public sealed class UIWidgetOptions : SerializedMonoBehaviour
    {
        [LabelText("名称")]
        [ReadOnly]
        public string WidgetName;

        [BoxGroup("设置", centerLabel: true)]
        [LabelText("类型")]
        [ReadOnly]
        public UIWidgetType Type = UIWidgetType.Default;

        [BoxGroup("设置", centerLabel: true)]
        [LabelText("优先级")]
        [ReadOnly]
        public int Priority;
    }
}