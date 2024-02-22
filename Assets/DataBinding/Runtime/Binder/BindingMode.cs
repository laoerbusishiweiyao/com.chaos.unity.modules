using Sirenix.OdinInspector;

namespace UnityEngine
{
    /// <summary>
    /// 绑定模式 - https://learn.microsoft.com/zh-cn/dotnet/api/system.windows.data.bindingmode
    /// </summary>
    [LabelText("绑定模式")]
    public enum BindingMode
    {
        Default,
        OneTime,
        OneWay,
        OneWayToSource,
        TwoWay,
    }
}