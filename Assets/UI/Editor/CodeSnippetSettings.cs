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
    public sealed class UISettings
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