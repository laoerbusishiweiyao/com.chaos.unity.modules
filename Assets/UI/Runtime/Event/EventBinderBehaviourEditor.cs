#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace UnityEngine
{
    public abstract partial class EventBinderBehaviour
    {
        [PropertySpace]
        [OdinSerialize]
        [LabelText("事件参数")]
        [PropertyOrder(98)]
        [DictionaryDrawerSettings(KeyLabel = "类型", ValueLabel = "名称", IsReadOnly = false,
            DisplayMode = DictionaryDisplayOptions.OneLine)]
        protected Dictionary<string, string> parameters = new();

        public IReadOnlyDictionary<string, string> Parameters => this.parameters;
    }
}
#endif