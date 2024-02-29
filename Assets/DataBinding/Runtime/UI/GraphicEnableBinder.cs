using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace UnityEngine
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Graphic))]
    [AddComponentMenu("DataBinding/GraphicEnableBinder")]
    public sealed class GraphicEnableBinder : DataBinderBehaviour
    {
        public override List<Type> PropertyTypes { get; } = new()
        {
            typeof(bool),
        };
        
        [LabelText("取反")]
        public bool Negation;

        [LabelText("目标")]
        [ReadOnly]
        public Graphic Target;

        public override void Refresh()
        {
            if (this.dataSource is null || this.dataSource.DataContext is null || this.Target is null)
            {
                this.Target.enabled = true;
                return;
            }

            if (this.binders.Count == 0)
            {
                this.Target.enabled = true;
                return;
            }

            bool enabled = this.GetValue(this.FirstDataBinder());
            this.Target.enabled = Negation ? !enabled : enabled;
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
            this.Target ??= this.GetComponent<Graphic>();
        }
    }
}