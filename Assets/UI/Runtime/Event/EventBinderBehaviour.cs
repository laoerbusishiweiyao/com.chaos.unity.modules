using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace UnityEngine
{
    /// <summary>
    /// 事件绑定器基类
    /// </summary>
    [ExecuteAlways]
    public abstract partial class EventBinderBehaviour : SerializedMonoBehaviour
    {
        public static event Action<EventBinderBehaviour, string> EventHandler;

        [LabelText("是否为异步任务")]
        public bool IsTask;

        [HideInInspector]
        public bool IsTasking;

        [LabelText("事件名称")]
        public string EventName;

        [PropertySpace]
        [OdinSerialize]
        [LabelText("额外数据")]
        [PropertyOrder(99)]
        [DictionaryDrawerSettings(KeyLabel = "名称", ValueLabel = "默认值", IsReadOnly = false,
            DisplayMode = DictionaryDisplayOptions.OneLine)]
        protected Dictionary<string, string> extraDatas = new();

        public IReadOnlyDictionary<string, string> ExtraDatas => this.extraDatas;

        protected void OnEvent()
        {
            if (this.IsTask && this.IsTasking)
            {
                return;
            }

            EventHandler?.Invoke(this, this.EventName);
        }
    }
}