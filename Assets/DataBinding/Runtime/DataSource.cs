using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace UnityEngine
{
    [AddComponentMenu("DataBinding/DataSource")]
    public sealed partial class DataSource : SerializedMonoBehaviour
    {
        public Type DataContextType => this.DataContext?.GetType();

        [HideLabel] [BoxGroup("数据上下文", centerLabel: true)] [TypeFilter("DataContextTypes", DrawValueNormally = true)]
        public DataContext DataContext;

        [OdinSerialize]
        [LabelText("绑定关系")]
        [PropertyOrder(99)]
        [DictionaryDrawerSettings(KeyLabel = "数据路径", ValueLabel = "数据内容", IsReadOnly = true,
            DisplayMode = DictionaryDisplayOptions.ExpandedFoldout)]
        [ReadOnly]
        private Dictionary<string, List<DataBinderBehaviour>> binders = new();

        public void AddDataBinderBehaviour(string path, DataBinderBehaviour behaviour)
        {
            if (!this.binders.ContainsKey(path))
            {
                this.binders.Add(path, new List<DataBinderBehaviour>());
            }

            if (this.binders[path].Contains(behaviour))
            {
                Debug.LogError($"重复绑定 {path} - {behaviour}");
                return;
            }

            this.binders[path].Add(behaviour);

            if (this.DataContext is null)
            {
                return;
            }

            behaviour.Initialize();
            behaviour.Refresh();
        }

        public void RemoveDataBinderBehaviour(string path, DataBinderBehaviour behaviour)
        {
            if (!this.binders.ContainsKey(path))
            {
                return;
            }

            if (!this.binders[path].Contains(behaviour))
            {
                return;
            }

            this.binders[path].Remove(behaviour);

            if (this.binders[path].Count == 0)
            {
                this.binders.Remove(path);
            }
        }

        public void Initialize(DataContext dataContext)
        {
            this.DataContext = dataContext;
            this.DataContext.AddListener(this.OnDataContextChanged);

            this.Refresh();
        }

        public void Refresh()
        {
            List<DataBinderBehaviour> behaviours = new();
            foreach (List<DataBinderBehaviour> components in this.binders.Values)
            {
                foreach (DataBinderBehaviour behaviour in components)
                {
                    if (behaviours.Contains(behaviour))
                    {
                        continue;
                    }

                    behaviours.Add(behaviour);
                }
            }

            foreach (DataBinderBehaviour behaviour in behaviours)
            {
                behaviour.Initialize();
                behaviour.Unbind();
                behaviour.Bind();
                behaviour.Refresh();
            }
        }

        private void OnDataContextChanged(object sender, DataContextChangedEventArgs eventArgs)
        {
            if (!this.binders.TryGetValue(eventArgs.Path, out List<DataBinderBehaviour> behaviours))
            {
                Debug.LogError($"数据{eventArgs.Path}未关联绑定组件");
                return;
            }

            foreach (DataBinderBehaviour behaviour in behaviours)
            {
                behaviour.Refresh();
            }
        }
    }
}