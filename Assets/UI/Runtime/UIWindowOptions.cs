using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace UnityEngine
{
    [AddComponentMenu("")]
    public sealed class UIWindowOptions : SerializedMonoBehaviour
    {
        [BoxGroup("节点数据", centerLabel: true)]
        [LabelText("默认加载控件集合")]
        [ListDrawerSettings(HideAddButton = true, DraggableItems = false)]
        [ReadOnly]
        public List<RectTransform> DefaultLoadedWidgets = new();

        [BoxGroup("节点数据", centerLabel: true)]
        [BoxGroup("节点数据/控件", centerLabel: true)]
        [LabelText("所有控件父节点")]
        [ReadOnly]
        public RectTransform WidgetParent;

        [BoxGroup("节点数据", centerLabel: true)]
        [BoxGroup("节点数据/控件", centerLabel: true)]
        [LabelText("控件集合")]
        [DictionaryDrawerSettings(
            KeyLabel = "名称",
            ValueLabel = "定位点",
            IsReadOnly = true,
            DisplayMode = DictionaryDisplayOptions.OneLine)]
        [ReadOnly]
        public Dictionary<string, RectTransform> AllWidget = new();

        [BoxGroup("节点数据", centerLabel: true)]
        [BoxGroup("节点数据/弹窗", centerLabel: true)]
        [LabelText("所有弹窗父节点")]
        [ReadOnly]
        public RectTransform PopupParent;

        [BoxGroup("节点数据", centerLabel: true)]
        [BoxGroup("节点数据/弹窗", centerLabel: true)]
        [LabelText("弹窗集合")]
        [DictionaryDrawerSettings(
            KeyLabel = "名称",
            ValueLabel = "定位点",
            IsReadOnly = true,
            DisplayMode = DictionaryDisplayOptions.OneLine)]
        [ReadOnly]
        public Dictionary<string, RectTransform> AllPopup = new();
    }
}