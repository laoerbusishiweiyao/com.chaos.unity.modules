using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace UnityEngine
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(GridLayoutGroup))]
    [AddComponentMenu("DataBinding/GridLayoutGroupStartCornerBinder")]
    public sealed class GridLayoutGroupStartCornerBinder : DataBinderBehaviour
    {
        public override List<Type> PropertyTypes { get; } = new()
        {
            typeof(int),
        };

        [LabelText("目标")]
        [ReadOnly]
        public GridLayoutGroup Target;

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

            this.SetColor();
        }

        private void SetColor()
        {
            var binder = this.FirstDataBinder();

            if (binder.DataType == typeof(int))
            {
                var result = this.dataSource.DataContext.GetValue<int>(binder.Source);
                var values = Enum.GetValues(typeof(GridLayoutGroup.Corner));
                foreach (int value in values)
                {
                    if (result != value)
                    {
                        continue;
                    }

                    this.Target.startCorner = (GridLayoutGroup.Corner)value;
                    return;
                }
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            this.Target ??= this.GetComponent<GridLayoutGroup>();
        }
    }
}