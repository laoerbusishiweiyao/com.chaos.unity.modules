using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace UnityEngine
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Graphic))]
    [AddComponentMenu("DataBinding/GraphicColorBinder")]
    public sealed class GraphicColorBinder : DataBinderBehaviour
    {
        public override List<Type> PropertyTypes { get; } = new()
        {
            typeof(List<float>),
            typeof(List<byte>),
        };

        [LabelText("目标")]
        [ReadOnly]
        public Graphic Target;

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

            if (binder.DataType == typeof(List<float>))
            {
                var rgba = this.dataSource.DataContext.GetValue<List<float>>(binder.Source);
                if (rgba.Count == 3)
                {
                    this.Target.color = new Color(rgba[0], rgba[1], rgba[2]);
                }

                if (rgba.Count == 4)
                {
                    this.Target.color = new Color(rgba[0], rgba[1], rgba[2], rgba[3]);
                }
            }

            if (binder.DataType == typeof(List<byte>))
            {
                var rgba = this.dataSource.DataContext.GetValue<List<byte>>(binder.Source);
                if (rgba.Count == 3)
                {
                    this.Target.color = new Color32(rgba[0], rgba[1], rgba[2], 255);
                }

                if (rgba.Count == 4)
                {
                    this.Target.color = new Color32(rgba[0], rgba[1], rgba[2], rgba[3]);
                }
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            this.Target ??= this.GetComponent<Graphic>();
        }
    }
}