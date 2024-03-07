using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace UnityEngine
{
    [LabelText("Image禁用行为")]
    public enum ImageDisableBehaviour
    {
        [LabelText("无")]
        None,

        [LabelText("禁用Image组件")]
        DisableComponent,

        [LabelText("设置Alpha为零")]
        SetAlphaZero,
    }

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

        [LabelText("设置为原始尺寸")]
        public bool SetNativeSize;

        public ImageDisableBehaviour DisableBehaviour = ImageDisableBehaviour.None;

        [LabelText("资源")]
        [ReadOnly]
        public string Location;

        [LabelText("目标")]
        [ReadOnly]
        public Image Target;

        public override void Refresh()
        {
            if (this.dataSource is null || this.dataSource.DataContext is null || this.Target is null)
            {
                this.Target.enabled = true;
                this.Target.CrossFadeAlpha(0f, 0f, true);
                return;
            }

            if (this.binders.Count == 0)
            {
                this.Target.enabled = true;
                this.Target.CrossFadeAlpha(0f, 0f, true);
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
                return;
            }

            this.Location = default;
        }

        private void SetSprite()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                if (string.IsNullOrEmpty(this.Location))
                {
                    this.Target.sprite = default;
                    switch (this.DisableBehaviour)
                    {
                        case ImageDisableBehaviour.DisableComponent:
                        {
                            this.Target.enabled = false;
                            break;
                        }
                        case ImageDisableBehaviour.SetAlphaZero:
                        {
                            this.Target.CrossFadeAlpha(0f, 0f, true);
                            break;
                        }
                        case ImageDisableBehaviour.None:
                        default:
                        {
                            break;
                        }
                    }
                }
                else
                {
                    Sprite sprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(this.Location);
                    this.Target.sprite = sprite;

                    if (sprite is null)
                    {
                        switch (this.DisableBehaviour)
                        {
                            case ImageDisableBehaviour.DisableComponent:
                            {
                                this.Target.enabled = false;
                                break;
                            }
                            case ImageDisableBehaviour.SetAlphaZero:
                            {
                                this.Target.CrossFadeAlpha(0f, 0f, true);
                                break;
                            }
                            case ImageDisableBehaviour.None:
                            default:
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        this.Target.enabled = true;
                        this.Target.CrossFadeAlpha(1f, 0f, true);
                    }

                    if (this.Target.enabled && this.SetNativeSize)
                    {
                        this.Target.SetNativeSize();
                    }
                }
            }
#endif
            
            OnImageSpriteChanged?.Invoke(this);
        }

        public override void Initialize()
        {
            base.Initialize();
            this.Target ??= this.GetComponent<Image>();
        }
    }
}