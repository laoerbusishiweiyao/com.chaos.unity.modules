using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine.Serialization;

namespace UnityEngine
{
    /// <summary>
    /// 事件绑定器基类
    /// </summary>
    [ExecuteAlways]
    public abstract partial class EventBinderBehaviour : SerializedMonoBehaviour
    {
        public static event Action<string, Dictionary<string, string>> EventHandler;

        [LabelText("事件名称")]
        public string EventName;

        [PropertySpace]
        [OdinSerialize]
        [LabelText("事件参数默认值")]
        [PropertyOrder(99)]
        [DictionaryDrawerSettings(KeyLabel = "名称", ValueLabel = "默认值", IsReadOnly = false,
            DisplayMode = DictionaryDisplayOptions.OneLine)]
        protected Dictionary<string, string> defaultValues = new();

        public IReadOnlyDictionary<string, string> DefaultValues => this.defaultValues;

        protected void OnEvent()
        {
            EventHandler?.Invoke(this.EventName, this.defaultValues);
        }
    }
}