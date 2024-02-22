#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace UnityEngine
{
    public sealed partial class DataSource
    {
        [ButtonGroup]
        [Button("删除数据上下文", ButtonSizes.Large)]
        [GUIColor(0.4f, 0.8f, 1)]
        private void DeleteDataContext()
        {
            if (this.DataContext is null)
            {
                return;
            }

            this.DataContext = null;
        }

        [ButtonGroup]
        [Button("清理绑定关系", ButtonSizes.Large)]
        [GUIColor(0.4f, 0.8f, 1)]
        private void Clean()
        {
            foreach (List<DataBinderBehaviour> behaviours in this.binders.Values)
            {
                behaviours.RemoveAll(behaviour => behaviour is null);
            }
        }

        private List<Type> DataContextTypes()
        {
            return DataContextOptions.Default.DataContextTypes;
        }

        private void OnValidate()
        {
            this.Refresh();
        }
    }
}
#endif