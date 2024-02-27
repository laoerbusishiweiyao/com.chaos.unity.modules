using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UnityEditor
{
    public sealed class CodeSnippetSettings : SerializedScriptableObject
    {
        public static CodeSnippetSettings Load()
        {
            string path = $"Assets/Editor/{nameof(CodeSnippetSettings)}.asset";

            string directory = Path.GetDirectoryName(path);
            if (directory is not null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            CodeSnippetSettings options = AssetDatabase.LoadAssetAtPath<CodeSnippetSettings>(path);
            if (options is null)
            {
                options = CreateInstance<CodeSnippetSettings>();
                AssetDatabase.CreateAsset(options, path);
                AssetDatabase.Refresh();
            }

            options.AddAllDefaultTemplate();

            return options;
        }

        [SerializeField]
        [HideInInspector]
        private List<CodeTemplate> templates = new();

        public List<CodeTemplate> Templates => this.templates;

        public List<string> AllTemplatePath => this.templates.Select(template => template.Path).ToList();

        [BoxGroup("模板", centerLabel: true)]
        [LabelText("名称")]
        public string TemplatePath;

        [BoxGroup("模板", centerLabel: true)]
        [HorizontalGroup("模板/操作")]
        [Button("删除", ButtonSizes.Large)]
        [GUIColor(0.4f, 0.8f, 1)]
        private void Delete()
        {
            if (string.IsNullOrEmpty(this.TemplatePath))
            {
                Debug.LogError("模板路径不能为空");
                return;
            }

            if (!this.AllTemplatePath.Contains(this.TemplatePath))
            {
                UIToolWindow.Notification("模板路径不存在");
                Debug.LogError("模板路径不存在");
                return;
            }

            CodeTemplate template = this.templates.Find(template => template.Path == this.TemplatePath);
            this.templates.Remove(template);

            UIToolWindow.Remove(this.TemplatePath);
            UIToolWindow.Rebuild("CodeSnippet");

            this.TemplatePath = string.Empty;
        }

        [BoxGroup("模板", centerLabel: true)]
        [HorizontalGroup("模板/操作")]
        [Button("添加", ButtonSizes.Large)]
        [GUIColor(0.4f, 0.8f, 1)]
        private void AddTemplate()
        {
            if (string.IsNullOrEmpty(this.TemplatePath))
            {
                Debug.LogError("模板路径不能为空");
                return;
            }

            if (this.AllTemplatePath.Contains(this.TemplatePath))
            {
                Debug.LogError("模板路径不能重复");
                return;
            }

            CodeTemplate template = this.AddTemplate($"CodeSnippet/{this.TemplatePath}");

            AssetDatabase.SaveAssets();

            UIToolWindow.Rebuild(template.Path);

            this.TemplatePath = string.Empty;
        }

        public void Build(string key, Dictionary<string, string> variables, string path)
        {
            CodeTemplate template = this.templates.Find(template => template.Path == key);
            if (template is null)
            {
                Debug.LogError($"代码模板 {key} 不存在");
                return;
            }

            StringBuilder builder = new();
            template.Build(variables, builder);
            
            string directory = Path.GetDirectoryName(path);
            if (directory is not null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(path, builder.ToString());
        }

        public void Build(string key, Dictionary<string, string> variables, StringBuilder builder)
        {
            CodeTemplate template = this.templates.Find(template => template.Path == key);
            if (template is null)
            {
                Debug.LogError($"代码模板 {key} 不存在");
                return;
            }

            template.Build(variables, builder, false);
        }

        private CodeTemplate AddTemplate(string path, string content = null)
        {
            CodeTemplate template = new()
            {
                Path = path,
                Template = content,
            };
            template.UpdateKeywords();
            this.templates.Add(template);

            return template;
        }

        public static readonly Dictionary<string, string> DefaultTemplates = new()
        {
            {
                "CodeSnippet/UISettings", @"namespace ET
{
    public static class UISettings
    {
        public const string ProjectName = ""$ProjectName$"";
        public const string Prefix = ""UI"";
        public const string NameSpace = ""$NameSpace$"";
        public const string AssetsFolder = ""$AssetsFolder$"";
        public const string OutputFolder = ""$OutputFolder$"";
        public const string ComponentFolder = ""$ComponentFolder$"" + ""/"" + ProjectName;
        public const string ComponentGenerateFolder = ""$ComponentFolder$"" + ""/"" + ProjectName + ""/Generate"";
        public const string SystemFolder = ""$ComponentSystemFolder$"" + ""/"" + ProjectName;
        public const string SystemGenerateFolder = ""$ComponentSystemFolder$"" + ""/"" + ProjectName + ""/Generate"";

        public const string SourceName = ""Source"";
        public const string WindowName = ""Window"";
        public const string PopupName = ""Popup"";
        public const string WidgetName = ""Widget"";

        public const string SourceFolderName = ""Source"";
        public const string PrefabsFolderName = ""Prefabs"";
        public const string SpritesFolderName = ""Sprites"";
        public const string AtlasFolderName = ""Atlas"";

        public const string WindowSourceNameFormat = Prefix + ""{0}"" + SourceName;
        public const string WindowNameFormat = Prefix + ""{0}"" + WindowName;
        public const string WidgetNameFormat = Prefix + ""{0}"" + WidgetName + ""{1}"";
        public const string PopupNameFormat = Prefix + ""{0}"" + PopupName + ""{1}"";

        public const int DesignScreenWidth = $ScreenDesignWidth$;
        public const int DesignScreenHeight = $ScreenDesignHeight$;
        public const float DesignScreenWidth_F = $ScreenDesignWidth$f;
        public const float DesignScreenHeight_F = $ScreenDesignHeight$f;
    }
}"
            },
            {
                "CodeSnippet/UIConfigCategory", @"using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [Code]
    public sealed class UIConfigCategory : Singleton<UIConfigCategory>, ISingletonAwake
    {
        private readonly Dictionary<Type, UIWindowConfig> configs = new()
        {
$Data$
        };

        public UIWindowConfig Config<TUIWindow>() where TUIWindow : IUIWindow
        {
            return this.configs[typeof(TUIWindow)];
        }

        public UIWidgetConfig Config<TUIWindow, TUIWidget>() where TUIWindow : IUIWindow where TUIWidget : IUIWidget
        {
            return this.configs[typeof(TUIWindow)].Widgets[typeof(TUIWidget)];
        }

        public void Awake()
        {
        }
    }
}"
            },
            {
                "CodeSnippet/UIWindow", @"using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(UIComponent))]
    public sealed partial class UI$Name$WindowComponent : Entity, IAwake, IDestroy, IUIWindow, IDataContext
    {
        public DataContext DataContext { get; } = new $Name$DataContext();

        public GameObject GameObject { get; set; }
        public UIWindowOptions Options { get; set; }
    }
}"
            },
            {
                "CodeSnippet/UIWidget", @"using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(UI$Parent$WindowComponent))]
    public sealed partial class UI$Parent$$Type$$Name$Component : Entity, IAwake, IDestroy, IUIWidget
    {
        public $Parent$$Name$$Type$DataContext DataContext => (this.GetParent<UI$Parent$WindowComponent>().DataContext as $Parent$DataContext).$Name$$Type$;
        public GameObject GameObject { get; set; }
        public UIWidgetOptions Options { get; set; }
    }
}"
            },
            {
                "CodeSnippet/UISystem", @"namespace ET.Client
{
    [EntitySystemOf(typeof(UI$Name$Component))]
    [FriendOf(typeof(UI$Name$Component))]
    public static partial class UI$Name$ComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UI$Name$Component self)
        {
        }

        [EntitySystem]
        private static void Destroy(this UI$Name$Component self)
        {
            UnityEngine.Object.Destroy(self.GameObject);
        }
    }
}"
            },
            {
                "CodeSnippet/UIDataContext", @"using UnityEngine;
using Sirenix.OdinInspector;

namespace ET.Client
{
    [HideReferenceObjectPicker]
    [EnableClass]
    public sealed partial class $Name$DataContext : DataContext
    {
        public $Name$DataContext()
        {
$Statement$
        }

        public $Name$DataContext(string baseDataContextPath = null) : base(baseDataContextPath)
        {
$Statement$
        }

$Properties$
    }
}"
            },
            {
                "CodeSnippet/UIDataContextProperty", @"        [SerializeField]
        [OnValueChanged(nameof(On$大写属性名称$Changed))]
        [Delayed]
        private $属性类型$ $小写字段名称$;

        public $属性类型$ $大写属性名称$
        {
            get => this.$小写字段名称$;
            set { this.SetField(ref this.$小写字段名称$, value); }
        }

        private void On$大写属性名称$Changed()
        {
            this.OnDataContextChanged(nameof(this.$大写属性名称$));
        }

"
            },
            {
                "CodeSnippet/UIEventType", @"using System.Collections.Generic;

namespace ET.Client
{
$EventType$
}"
            },
            {
                "CodeSnippet/UIEventSystem", @"using System;
using System.Collections.Generic;

namespace ET.Client
{
    public sealed partial class UIEventSystem
    {
        private readonly Dictionary<string, KeyValuePair<Type, Type>> unityEventTable = new()
        {
$Map$
        };

        public void OnEvent(UIComponent uiComponent, string eventName, Dictionary<string, string> extraDatas)
        {
            if (!this.unityEventTable.TryGetValue(eventName, out var pair) || !this.allUIEvents.TryGetValue(pair.Key, out var uiHandlers) ||
                !uiHandlers.TryGetValue(pair.Value, out var handlers))
            {
                return;
            }

            foreach (IUIEvent ui in handlers)
            {
                if (ui is not IUIEventHandler handler)
                {
                    Log.Error($""event error: {ui.EventArgsType.FullName}"");
                    continue;
                }

                Entity entity = uiComponent.GetComponent(pair.Key);
                object args = Activator.CreateInstance(pair.Value);
                handler.Handle(entity, args, extraDatas).Coroutine();
            }
        }
    }
}"
            },
        };

        public void AddAllDefaultTemplate()
        {
            foreach ((string path, string content) in DefaultTemplates)
            {
                if (this.AllTemplatePath.Contains(path))
                {
                    continue;
                }

                this.AddTemplate(path, content);
            }
        }
    }
}