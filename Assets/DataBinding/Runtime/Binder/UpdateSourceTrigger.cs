using Sirenix.OdinInspector;

namespace UnityEngine
{
    /// <summary>
    /// 数据源更新触发方式 - https://learn.microsoft.com/zh-cn/dotnet/api/system.windows.data.updatesourcetrigger
    /// </summary>
    [LabelText("触发方式")]
    public enum UpdateSourceTrigger
    {
        Default,
        Explicit,
        LostFocus,
        PropertyChanged,
    }
}