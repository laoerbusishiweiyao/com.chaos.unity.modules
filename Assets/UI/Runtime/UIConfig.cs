using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace UnityEngine
{
    [HideLabel]
    [HideReferenceObjectPicker]
    [Serializable]
    public sealed class UIConfig
    {
        [LabelText("名称")]
        [ReadOnly]
        public string Name;

        public UIType Type = UIType.Window;

        public UILayer Layer = UILayer.Common;

        [LabelText("资源路径")]
        [ReadOnly]
        public string Asset;

        [ShowInInspector]
        [LabelText("控件")]
        [DictionaryDrawerSettings(
            KeyLabel = "控件名称",
            ValueLabel = "控件资源路径",
            DisplayMode = DictionaryDisplayOptions.OneLine)]
        [ReadOnly]
        public Dictionary<string, string> Controls = new();
    }
}