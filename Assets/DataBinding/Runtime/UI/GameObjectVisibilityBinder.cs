using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace UnityEngine
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Graphic))]
    [AddComponentMenu("DataBinding/GameObjectVisibilityBinder")]
    public sealed class GameObjectVisibilityBinder : DataBinderBehaviour
    {
        public override List<Type> PropertyTypes { get; } = new()
        {
            typeof(bool),
        };

        [LabelText("取反")]
        public bool Negation;

        [LabelText("目标")]
        [ReadOnly]
        public GameObject Target;

        public override void Refresh()
        {
            if (this.dataSource is null || this.dataSource.DataContext is null || this.Target is null)
            {
                return;
            }

            if (this.binders.Count == 0)
            {
                return;
            }

            bool visibility = this.GetValue(this.FirstDataBinder());
            this.Target.SetActive(Negation ? !visibility : visibility);
        }

        private bool GetValue(DataBinder binder)
        {
            if (binder.DataType == typeof(bool))
            {
                return this.dataSource.DataContext.GetValue<bool>(binder.Source);
            }

            return true;
        }

        public override void Initialize()
        {
            base.Initialize();
            this.Target ??= this.gameObject;
        }
    }
}