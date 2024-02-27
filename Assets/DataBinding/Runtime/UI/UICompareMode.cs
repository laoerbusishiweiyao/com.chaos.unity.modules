using Sirenix.OdinInspector;

namespace UnityEngine
{
    [LabelText("比较运算")]
    public enum UICompareMode
    {
        [LabelText("小于")]
        Less,

        [LabelText("小于等于")]
        LessEqual,

        [LabelText("等于")]
        Equal,

        [LabelText("大于")]
        Great,

        [LabelText("大于等于")]
        GreatEqual,

        [LabelText("不等于")]
        NotEqual,
    }
}