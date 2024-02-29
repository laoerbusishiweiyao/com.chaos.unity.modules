#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;

namespace UnityEngine
{
    public abstract partial class DataBinderBehaviour
    {
        [PropertySpace]
        [SerializeField]
        [ValueDropdown(nameof(SourcePaths))]
        [LabelText("路径")]
        [PropertyOrder(-3)]
        [DisableInPlayMode]
        private string path = string.Empty;

        [ButtonGroup("操作面板", -2)]
        [GUIColor(0.4f, 0.8f, 1)]
        [Button("添加", ButtonSizes.Medium)]
        private void AddDataBinder()
        {
            if (string.IsNullOrEmpty(this.path))
            {
                Debug.LogError("未选择数据路径");
                return;
            }

            if (this.binders.ContainsKey(this.path))
            {
                Debug.LogError("数据路径已存在");
                return;
            }

            DataBinder binder = new()
            {
                Source = this.path, DataType = DataContextCache.PropertyType(this.dataSource.DataContextType, this.path)
            };

            this.binders.Add(this.path, binder);
            this.Bind(binder);
            this.path = string.Empty;
        }

        [ButtonGroup("操作面板", -2)]
        [GUIColor(1, 0.6f, 0.4f)]
        [Button("移除", ButtonSizes.Medium)]
        private void RemoveDataBinder()
        {
            if (string.IsNullOrEmpty(this.path))
            {
                Debug.LogError("未选择数据路径");
                return;
            }

            if (!this.binders.ContainsKey(this.path))
            {
                Debug.LogError("数据路径不存在");
                return;
            }

            DataBinder binder = this.binders[this.path];

            this.binders.Remove(this.path);

            this.Unbind(binder);

            this.path = string.Empty;
        }

        [ButtonGroup("操作面板", -2)]
        [Button("检查", ButtonSizes.Medium)]
        private void CheckDataBinder()
        {
        }

        [ButtonGroup("操作面板", -2)]
        [GUIColor(1, 0, 0)]
        [Button("清空", ButtonSizes.Medium)]
        private void ClearDataBinder()
        {
            this.path = string.Empty;

            this.Unbind();

            this.binders.Clear();
        }

        private List<string> SourcePaths() => this.dataSource is null || this.dataSource.DataContext is null
            ? new List<string>()
            : DataContextCache.FilterPropertyPath(this.dataSource.DataContextType, this.PropertyTypes);

        protected virtual void OnValidate()
        {
            this.Initialize();
            this.Unbind();
            this.Bind();
            this.Refresh();
        }
    }
}
#endif