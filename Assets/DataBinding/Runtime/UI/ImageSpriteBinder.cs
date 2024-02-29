using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine.UI;

namespace UnityEngine
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Image))]
    [AddComponentMenu("DataBinding/ImageSpriteBinder")]
    public sealed class ImageSpriteBinder : DataBinderBehaviour
    {
        public static event Action<ImageSpriteBinder> OnImageSpriteChanged;
        
        public override List<Type> PropertyTypes { get; } = new()
        {
            typeof(string),
        };

        [LabelText("禁用Image组件当Sprite为null")]
        public bool DisableWhenNull = true;

        [LabelText("资源")]
        [ReadOnly]
        public string Location;

        [LabelText("目标")]
        [ReadOnly]
        public Image Target;

        public override void Refresh()
        {
            if (this.dataSource is null || this.dataSource.DataContext is null)
            {
                this.Target.enabled = true;
                return;
            }

            if (this.binders.Count == 0)
            {
                this.Target.enabled = true;
                return;
            }

            this.SetLocation();
            this.SetSprite();
        }

        private void SetLocation()
        {
            DataBinder binder = this.FirstDataBinder();
            if (binder.DataType == typeof(string))
            {
                this.Location = this.dataSource.DataContext.GetValue<string>(binder.Source);
            }

            this.Location = default;
        }

        private void SetSprite()
        {
            if (!Application.isPlaying)
            {
                if (string.IsNullOrEmpty(this.Location))
                {
                    this.Target.sprite = default;
                    if (DisableWhenNull)
                    {
                        this.Target.enabled = false;
                    }
                }
                else
                {
                    Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(this.Location);
                    this.Target.sprite = sprite;
                    this.Target.enabled = sprite is not null;
                }
            }

            OnImageSpriteChanged?.Invoke(this);
        }

        public override void Initialize()
        {
            base.Initialize();
            this.Target ??= this.GetComponent<Image>();
        }
    }
}