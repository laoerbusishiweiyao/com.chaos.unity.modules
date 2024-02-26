using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Serialization;

namespace UnityEditor
{
    public sealed class UIToolSettings : SerializedScriptableObject
    {
        public static UIToolSettings Load()
        {
            string path = $"Assets/Editor/{nameof(UIToolSettings)}.asset";

            string directory = Path.GetDirectoryName(path);
            if (directory is not null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            UIToolSettings options = AssetDatabase.LoadAssetAtPath<UIToolSettings>(path);
            if (options is null)
            {
                options = CreateInstance<UIToolSettings>();
                AssetDatabase.CreateAsset(options, path);
                AssetDatabase.Refresh();
            }

            return options;
        }

        [BoxGroup("设置", centerLabel: true)]
        [BoxGroup("设置/基本设置", centerLabel: true)]
        [LabelText("UI项目名称")]
        public string ProjectName = "Chaos";

        [BoxGroup("设置", centerLabel: true)]
        [BoxGroup("设置/基本设置", centerLabel: true)]
        [LabelText("资源路径")]
        [FolderPath]
        public string AssetsFolder = "Assets/Res/UI";

        [BoxGroup("设置", centerLabel: true)]
        [BoxGroup("设置/基本设置", centerLabel: true)]
        [LabelText("发布路径")]
        [FolderPath]
        public string OutputFolder = "Assets/Bundles/UI";

        [BoxGroup("设置", centerLabel: true)]
        [BoxGroup("设置/代码生成设置", centerLabel: true)]
        [LabelText("命名空间")]
        public string NameSpace = "ET.Client";

        [BoxGroup("设置", centerLabel: true)]
        [BoxGroup("设置/代码生成设置", centerLabel: true)]
        [LabelText("组件路径")]
        [FolderPath]
        public string ComponentFolder = "Assets/Scripts/ModelView/Client/UI";

        [BoxGroup("设置", centerLabel: true)]
        [BoxGroup("设置/代码生成设置", centerLabel: true)]
        [LabelText("组件系统路径")]
        [FolderPath]
        public string ComponentSystemFolder = "Assets/Scripts/HotfixView/Client/UI";

        [BoxGroup("设置", centerLabel: true)]
        [BoxGroup("设置/屏幕设置", centerLabel: true)]
        [LabelText("屏幕设计宽度")]
        public int ScreenDesignWidth = 1920;

        [BoxGroup("设置", centerLabel: true)]
        [BoxGroup("设置/屏幕设置", centerLabel: true)]
        [LabelText("屏幕设计高度")]
        public int ScreenDesignHeight = 1080;

        [BoxGroup("设置", centerLabel: true)]
        [LabelText("UI设置代码生成文件夹")]
        [FolderPath]
        public string SettingsFolderPath = "Assets/Scripts/ModelView/Client/Module/UI";

        [Button("初始化", ButtonSizes.Gigantic)]
        [GUIColor(0.4f, 0.8f, 1)]
        private void BuildUISettings()
        {
            if (string.IsNullOrEmpty(this.ProjectName))
            {
                Debug.LogError("UI项目名称不能为空");
                return;
            }

            if (string.IsNullOrEmpty(this.AssetsFolder))
            {
                Debug.LogError("资源路径不能为空");
                return;
            }

            if (string.IsNullOrEmpty(this.OutputFolder))
            {
                Debug.LogError("发布路径不能为空");
                return;
            }

            if (string.IsNullOrEmpty(this.NameSpace))
            {
                Debug.LogError("命名空间不能为空");
                return;
            }

            if (string.IsNullOrEmpty(this.ComponentFolder))
            {
                Debug.LogError("组件路径不能为空");
                return;
            }

            if (string.IsNullOrEmpty(this.ComponentSystemFolder))
            {
                Debug.LogError("组件系统路径不能为空");
                return;
            }

            if (this.ScreenDesignWidth < 1)
            {
                Debug.LogError("屏幕设计宽度异常");
                return;
            }

            if (this.ScreenDesignHeight < 1)
            {
                Debug.LogError("屏幕设计高度异常");
                return;
            }

            if (string.IsNullOrEmpty(this.SettingsFolderPath))
            {
                Debug.LogError("设置路径不能为空");
                return;
            }

            CodeSnippetSettings options = CodeSnippetSettings.Load();
            options.Build("CodeSnippet/UISettings", new Dictionary<string, string>()
                {
                    { nameof(ProjectName), this.ProjectName },
                    { nameof(NameSpace), this.NameSpace },
                    { nameof(AssetsFolder), this.AssetsFolder },
                    { nameof(OutputFolder), this.OutputFolder },
                    { nameof(ComponentFolder), this.ComponentFolder },
                    { nameof(ComponentSystemFolder), this.ComponentSystemFolder },
                    { nameof(ScreenDesignWidth), this.ScreenDesignWidth.ToString() },
                    { nameof(ScreenDesignHeight), this.ScreenDesignHeight.ToString() },
                }, $"{this.SettingsFolderPath}/UISettings.cs");

            AssetDatabase.Refresh();
        }

        [Button("生成", ButtonSizes.Gigantic)]
        [GUIColor(0.4f, 0.8f, 1)]
        private void Build()
        {
        }

        [FormerlySerializedAs("UIName")]
        [BoxGroup("创建UI", centerLabel: true, order: 99)]
        [LabelText("UI名称")]
        public string WindowName;

        [BoxGroup("创建UI", centerLabel: true, order: 99)]
        [ButtonGroup("创建UI/操作")]
        [Button("删除", ButtonSizes.Medium)]
        [GUIColor(0.4f, 0.8f, 1)]
        private void DeleteWindow()
        {
            if (string.IsNullOrEmpty(this.WindowName))
            {
                Debug.LogError("名称不能为空");
                return;
            }

            if (!this.Windows.Contains(this.WindowName))
            {
                Debug.LogError("窗口不存在");
                return;
            }

            UIWindowBuildSettings settings =
                this.windowSettings.Find(settings => settings.WindowName == this.WindowName);
            this.windowSettings.Remove(settings);

            this.WindowName = string.Empty;

            UIToolWindow.Rebuild();
            AssetDatabase.Refresh();
        }

        [BoxGroup("创建UI", centerLabel: true, order: 99)]
        [ButtonGroup("创建UI/操作")]
        [Button("创建", ButtonSizes.Medium)]
        [GUIColor(0.4f, 0.8f, 1)]
        private void CreateWindow()
        {
            if (string.IsNullOrEmpty(this.WindowName))
            {
                Debug.LogError("名称不能为空");
                return;
            }

            if (this.Windows.Contains(this.WindowName))
            {
                Debug.LogError("窗口已存在");
                return;
            }

            CreateAssetsDirectory(this.WindowName, "Source");
            CreateAssetsDirectory(this.WindowName, "Atlas");
            CreateAssetsDirectory(this.WindowName, "Prefabs");
            CreateAssetsDirectory(this.WindowName, "Sprites");

            GameObject source = CreateWindowPrefab(this.WindowName);

            UIWindowBuildSettings buildSettings = new UIWindowBuildSettings()
            {
                WindowName = this.WindowName, Path = $"UIBuilder/{this.WindowName}Window",
                Source = AssetDatabase.GetAssetPath(source)
            };
            this.WindowSettings.Add(buildSettings);

            this.WindowName = string.Empty;

            UIToolWindow.Rebuild();
            AssetDatabase.Refresh();
        }

        [SerializeField]
        [HideInInspector]
        private List<UIWindowBuildSettings> windowSettings = new();

        public List<UIWindowBuildSettings> WindowSettings => this.windowSettings;

        public List<string> Windows => this.windowSettings.Select(settings => settings.WindowName).ToList();

        private void CreateAssetsDirectory(string value, string folder)
        {
            string directory = string.Join('/', Application.dataPath[..^6], this.AssetsFolder, this.ProjectName, value,
                folder);
            if (directory is not null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        private void CreateOutputDirectory(string value)
        {
            string directory = string.Join('/', Application.dataPath[..^6], this.OutputFolder, this.ProjectName, value);
            if (directory is not null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        private GameObject CreateWindowPrefab(string value)
        {
            string path = string.Join('/', Application.dataPath[..^6], this.AssetsFolder, this.ProjectName, value,
                "Source",
                $"UI{value}Source.prefab");
            string directory = Path.GetDirectoryName(path);
            if (directory is not null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string name = Path.GetFileNameWithoutExtension(path);

            GameObject window = new(name, typeof(RectTransform), typeof(UIWindowEditorSettings));
            window.GetComponent<RectTransform>().SetFullScreen();

            GameObject background = new("BackgroundTrigger", typeof(RectTransform), typeof(UITrigger));
            background.transform.SetParent(window.transform);
            background.GetComponent<RectTransform>().SetFullScreen();

            GameObject control = new("Widget", typeof(RectTransform));
            control.transform.SetParent(window.transform);
            control.GetComponent<RectTransform>().SetFullScreen();

            GameObject popup = new("Popup", typeof(RectTransform));
            popup.transform.SetParent(window.transform);
            popup.GetComponent<RectTransform>().SetFullScreen();

            UIWindowEditorSettings options = window.GetComponent<UIWindowEditorSettings>();
            options.WindowName = value;
            options.WidgetParent = control.GetComponent<RectTransform>();
            options.PopupParent = popup.GetComponent<RectTransform>();

            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(window, path);
            DestroyImmediate(window);

            Selection.activeObject =
                PrefabStageUtility.OpenPrefab(AssetDatabase.GetAssetPath(prefab)).prefabContentsRoot;

            return prefab;
        }
    }
}