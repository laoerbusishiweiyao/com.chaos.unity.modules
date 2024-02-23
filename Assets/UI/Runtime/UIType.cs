using Sirenix.OdinInspector;

namespace UnityEngine
{
    [LabelText("类型")]
    public enum UIType
    {
        [LabelText("窗口")]
        Window,

        [LabelText("弹窗")]
        Popup,

        [LabelText("控件")]
        Widget,
    }
}