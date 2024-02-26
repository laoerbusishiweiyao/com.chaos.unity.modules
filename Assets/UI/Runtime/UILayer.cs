using Sirenix.OdinInspector;

namespace UnityEngine
{
    [LabelText("层级")]
    public enum UILayer
    {
        [LabelText("最高层")]
        Top = 0,

        [LabelText("提示层")]
        Tips = 1,

        [LabelText("弹窗层")]
        Popup = 2,

        [LabelText("常规层")]
        Window = 3,

        [LabelText("场景层")]
        Scene = 4,

        [LabelText("最低层")]
        Bottom = 5,

        [LabelText("")]
        Cache = 6,

        [LabelText("")]
        Count = 7,
    }
}