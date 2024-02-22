using System;
using Sirenix.OdinInspector;

namespace UnityEngine
{
    /// <summary>
    /// 数据绑定器
    /// </summary>
    [Serializable]
    [HideLabel]
    [HideReferenceObjectPicker]
    public sealed class DataBinder
    {
        /// <summary>
        /// 数据源 - 数据上下文中的属性路径
        /// </summary>
        [LabelText("源")] [ReadOnly] public string Source;

        /// <summary>
        /// 绑定模式
        /// </summary>
        public BindingMode Mode = BindingMode.Default;

        /// <summary>
        /// 数据源更新触发方式
        /// </summary>
        public UpdateSourceTrigger Trigger = UpdateSourceTrigger.Default;

        [LabelText("数据类型")] [ReadOnly] public Type DataType;
    }
}