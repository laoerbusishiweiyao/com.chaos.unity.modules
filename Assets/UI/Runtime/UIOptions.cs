using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine.Serialization;

namespace UnityEngine
{
    [AddComponentMenu("")]
    public sealed partial class UIOptions : SerializedMonoBehaviour
    {
        [LabelText("名称")]
        [ReadOnly]
        public string Name;

        [BoxGroup("设置", centerLabel: true)]
        [LabelText("类型")]
        [ReadOnly]
        public UIType Type = UIType.Window;

        [BoxGroup("设置", centerLabel: true)]
        [LabelText("层级")]
        [ShowIf(nameof(Type), UIType.Window)]
        public UILayer Layer = UILayer.Common;

        [BoxGroup("设置", centerLabel: true)]
        [LabelText("优先级")]
        public int Priority;
        
        [BoxGroup("节点数据", centerLabel: true)]
        [LabelText("默认加载控件集合")]
        [Tooltip("拖拽赋值")]
        [ListDrawerSettings(HideAddButton = true, DraggableItems = false)]
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

        // [BoxGroup("节点数据", centerLabel: true)]
        // [Button("刷新", ButtonSizes.Large)]
        // [GUIColor(0.4f, 0.8f, 1)]
        // private void UpdateAllLocator()
        // {
        //     this.AllControlLocator.Clear();
        //     for (int i = 0; i < this.ControlNode.childCount; i++)
        //     {
        //         RectTransform child = this.ControlNode.GetChild(i).GetComponent<RectTransform>();
        //         this.AllControlLocator.Add(child.name, child);
        //     }
        //
        //     this.AllPopupLocator.Clear();
        //     for (int i = 0; i < this.PopupNode.childCount; i++)
        //     {
        //         RectTransform child = this.PopupNode.GetChild(i).GetComponent<RectTransform>();
        //         this.AllPopupLocator.Add(child.name, child);
        //     }
        // }
    }
}