using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Sirenix.OdinInspector;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace UnityEditor
{
    [Serializable]
    public sealed class UIWindowBuildSettings
    {
        [LabelText("窗口名称")]
        [ReadOnly]
        public string WindowName;

        [HideInInspector]
        public string Path;

        [HideInInspector]
        public string Source;

        [Button("生成", ButtonSizes.Large)]
        [GUIColor(0.4f, 0.8f, 1)]
        public void Build()
        {
            this.BuildWindow();
            Debug.Log($"{this.WindowName} UI预制生成完毕");

            this.BuildEvent();
            Debug.Log($"{this.WindowName} UI事件生成完毕");

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void BuildWindow()
        {
            // 如果当前打开的是输出的预制体，则刷新
            var stage = PrefabStageUtility.GetCurrentPrefabStage();
            if (stage is not null)
            {
                Debug.LogError("请先退出预制体编辑模式");
                return;
            }

            UIToolSettings toolSettings = UIToolSettings.Load();

            string path = string.Join('/', toolSettings.OutputFolder, toolSettings.ProjectName, this.WindowName,
                $"UI{this.WindowName}Window.prefab");

            string directory = System.IO.Path.GetDirectoryName(path);
            if (directory is not null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            GameObject source = AssetDatabase.LoadAssetAtPath<GameObject>(this.Source);
            GameObject window = UnityEngine.Object.Instantiate(source);

            UIWindowOptions options = window.AddComponent<UIWindowOptions>();
            UIWindowEditorSettings settings = window.GetComponent<UIWindowEditorSettings>();

            this.BuildGenerateCode(settings);
            this.BuildCode(settings);

            this.Copy(settings, options);

            this.BuildWidget(options);

            // 清理控件
            foreach (var widget in options.AllWidget.Values)
            {
                widget.CleanImmediate();
            }

            // 清理弹窗
            foreach (var popup in options.AllPopup.Values)
            {
                popup.CleanImmediate();
            }

            // 加载默认加载控件
            foreach (RectTransform widget in options.DefaultLoadedWidgets)
            {
                if (options.AllWidget.ContainsValue(widget))
                {
                    string widgetPath = string.Join('/', toolSettings.OutputFolder, toolSettings.ProjectName,
                        this.WindowName, $"UI{this.WindowName}Widget{widget.name}.prefab");

                    PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(widgetPath), widget);
                }

                if (options.AllPopup.ContainsValue(widget))
                {
                    string widgetPath = string.Join('/', toolSettings.OutputFolder, toolSettings.ProjectName,
                        this.WindowName, $"UI{this.WindowName}Popup{widget.name}.prefab");

                    PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(widgetPath), widget);
                }
            }

            PrefabUtility.SaveAsPrefabAsset(window, path);
            UnityEngine.Object.DestroyImmediate(window);
        }

        private void BuildWidget(UIWindowOptions windowOptions)
        {
            UIToolSettings toolSettings = UIToolSettings.Load();

            foreach (string name in windowOptions.AllWidget.Keys)
            {
                string sourcePath = string.Join('/', toolSettings.AssetsFolder, toolSettings.ProjectName,
                    this.WindowName, "Prefabs", $"UI{this.WindowName}Widget{name}.prefab");

                if (!File.Exists(sourcePath))
                {
                    Debug.LogError($"窗口控件 {sourcePath} 不存在");
                    continue;
                }

                string outputPath = string.Join('/', toolSettings.OutputFolder, toolSettings.ProjectName,
                    this.WindowName, $"UI{this.WindowName}Widget{name}.prefab");
                string directory = System.IO.Path.GetDirectoryName(outputPath);
                if (directory is not null && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                GameObject source = AssetDatabase.LoadAssetAtPath<GameObject>(sourcePath);
                GameObject widget = UnityEngine.Object.Instantiate(source);

                UIWidgetOptions options = widget.AddComponent<UIWidgetOptions>();
                UIWidgetEditorSettings editorSettings = widget.GetComponent<UIWidgetEditorSettings>();
                this.Copy(editorSettings, options);

                UnityEngine.Object.DestroyImmediate(widget.GetComponent("DataSource"));

                PrefabUtility.SaveAsPrefabAsset(widget, outputPath);
                UnityEngine.Object.DestroyImmediate(widget);
            }

            foreach (string name in windowOptions.AllPopup.Keys)
            {
                string sourcePath = string.Join('/', toolSettings.AssetsFolder, toolSettings.ProjectName,
                    this.WindowName, "Prefabs", $"UI{this.WindowName}Popup{name}.prefab");
                if (!File.Exists(sourcePath))
                {
                    Debug.LogError($"窗口控件 {sourcePath} 不存在");
                    continue;
                }

                string outputPath = string.Join('/', toolSettings.OutputFolder, toolSettings.ProjectName,
                    this.WindowName, $"UI{this.WindowName}Popup{name}.prefab");
                string directory = System.IO.Path.GetDirectoryName(outputPath);
                if (directory is not null && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                GameObject source = AssetDatabase.LoadAssetAtPath<GameObject>(sourcePath);
                GameObject widget = UnityEngine.Object.Instantiate(source);

                UIWidgetOptions options = widget.AddComponent<UIWidgetOptions>();
                UIWidgetEditorSettings editorSettings = widget.GetComponent<UIWidgetEditorSettings>();
                this.Copy(editorSettings, options);

                UnityEngine.Object.DestroyImmediate(widget.GetComponent("DataSource"));

                PrefabUtility.SaveAsPrefabAsset(widget, outputPath);
                UnityEngine.Object.DestroyImmediate(widget);
            }
        }

        private void Copy(UIWindowEditorSettings settings, UIWindowOptions options)
        {
            options.DefaultLoadedWidgets.AddRange(settings.DefaultLoadedWidgets);
            options.WidgetParent = settings.WidgetParent;
            foreach ((string key, RectTransform value) in settings.AllWidget)
            {
                options.AllWidget.Add(key, value);
            }

            options.PopupParent = settings.PopupParent;
            foreach ((string key, RectTransform value) in settings.AllPopup)
            {
                options.AllPopup.Add(key, value);
            }

            UnityEngine.Object.DestroyImmediate(settings);
        }

        private void Copy(UIWidgetEditorSettings settings, UIWidgetOptions options)
        {
            UnityEngine.Object.DestroyImmediate(settings);
        }

        private void BuildGenerateCode(UIWindowEditorSettings windowSettings)
        {
            UIToolSettings toolSettings = UIToolSettings.Load();
            CodeSnippetSettings options = CodeSnippetSettings.Load();

            StringBuilder statementBuilder = new();
            StringBuilder windowDataContextPropertiesBuilder = new();

            options.Build("CodeSnippet/UIWindow", new Dictionary<string, string>()
                {
                    { "Name", windowSettings.WindowName },
                },
                $"{toolSettings.ComponentGenerateFolder}/{windowSettings.WindowName}/UI{windowSettings.WindowName}WindowComponent.cs");

            options.Build("CodeSnippet/UISystem", new Dictionary<string, string>()
                {
                    { "Name", $"{windowSettings.WindowName}Window" },
                },
                $"{toolSettings.SystemGenerateFolder}/{windowSettings.WindowName}/UI{windowSettings.WindowName}WindowComponentSystem.cs");

            foreach (var widget in windowSettings.AllWidget.Keys)
            {
                options.Build("CodeSnippet/UIWidget", new Dictionary<string, string>()
                    {
                        { "Parent", windowSettings.WindowName },
                        { "Name", widget },
                        { "Type", "Widget" },
                    },
                    $"{toolSettings.ComponentGenerateFolder}/{windowSettings.WindowName}/UI{windowSettings.WindowName}Widget{widget}Component.cs");

                options.Build("CodeSnippet/UISystem", new Dictionary<string, string>()
                    {
                        { "Name", $"{windowSettings.WindowName}Widget{widget}" },
                    },
                    $"{toolSettings.SystemGenerateFolder}/{windowSettings.WindowName}/UI{windowSettings.WindowName}Widget{widget}ComponentSystem.cs");

                options.Build("CodeSnippet/UIDataContext", new Dictionary<string, string>()
                    {
                        { "Name", $"{windowSettings.WindowName}{widget}Widget" },
                        { "Statement", "" },
                        { "Properties", "" },
                    },
                    $"{toolSettings.ComponentGenerateFolder}/{windowSettings.WindowName}/{windowSettings.WindowName}{widget}WidgetDataContext.cs");

                statementBuilder.AppendLine(
                    $"\t\t\tthis.{widget.ToLower()[0]}{widget[1..]}Widget = new(nameof({widget}Widget));");
                options.Build("CodeSnippet/UIDataContextProperty", new Dictionary<string, string>()
                {
                    { "大写属性名称", $"{widget}Widget" },
                    { "小写字段名称", $"{widget.ToLower()[0]}{widget[1..]}Widget" },
                    { "属性类型", $"{windowSettings.WindowName}{widget}WidgetDataContext" },
                }, windowDataContextPropertiesBuilder);
            }

            foreach (var widget in windowSettings.AllPopup.Keys)
            {
                options.Build("CodeSnippet/UIWidget", new Dictionary<string, string>()
                    {
                        { "Parent", windowSettings.WindowName },
                        { "Name", widget },
                        { "Type", "Popup" },
                    },
                    $"{toolSettings.ComponentGenerateFolder}/{windowSettings.WindowName}/UI{windowSettings.WindowName}Popup{widget}Component.cs");

                options.Build("CodeSnippet/UISystem", new Dictionary<string, string>()
                    {
                        { "Name", $"{windowSettings.WindowName}Popup{widget}" },
                    },
                    $"{toolSettings.SystemGenerateFolder}/{windowSettings.WindowName}/UI{windowSettings.WindowName}Popup{widget}ComponentSystem.cs");

                options.Build("CodeSnippet/UIDataContext", new Dictionary<string, string>()
                    {
                        { "Name", $"{windowSettings.WindowName}{widget}Popup" },
                        { "Statement", "" },
                        { "Properties", "" },
                    },
                    $"{toolSettings.ComponentGenerateFolder}/{windowSettings.WindowName}/{windowSettings.WindowName}{widget}PopupDataContext.cs");

                statementBuilder.AppendLine(
                    $"\t\t\tthis.{widget.ToLower()[0]}{widget[1..]}Popup = new(nameof({widget}Popup));");
                options.Build("CodeSnippet/UIDataContextProperty", new Dictionary<string, string>()
                {
                    { "大写属性名称", $"{widget}Popup" },
                    { "小写字段名称", $"{widget.ToLower()[0]}{widget[1..]}Popup" },
                    { "属性类型", $"{windowSettings.WindowName}{widget}PopupDataContext" },
                }, windowDataContextPropertiesBuilder);
            }

            options.Build("CodeSnippet/UIDataContext", new Dictionary<string, string>()
                {
                    { "Name", windowSettings.WindowName },
                    { "Statement", statementBuilder.ToString() },
                    { "Properties", windowDataContextPropertiesBuilder.ToString() },
                },
                $"{toolSettings.ComponentGenerateFolder}/{windowSettings.WindowName}/{windowSettings.WindowName}DataContext.cs");
        }

        private void BuildCode(UIWindowEditorSettings windowSettings)
        {
            // 生成用户UI - 待定
        }

        private void BuildEvent()
        {
            UIToolSettings toolSettings = UIToolSettings.Load();
            string sourcePath = string.Join('/', toolSettings.AssetsFolder, toolSettings.ProjectName, this.WindowName,
                "Source", $"UI{this.WindowName}Source.prefab");
            GameObject source = AssetDatabase.LoadAssetAtPath<GameObject>(sourcePath);
            var settings = source.GetComponent<UIWindowEditorSettings>();

            StringBuilder builder = new();

            List<string> eventNames = new();
            foreach (string key in settings.AllWidget.Keys)
            {
                string path = sourcePath.Replace("/Source/", "/Prefabs/")
                    .Replace("Source.prefab", $"Widget{key}.prefab");
                GameObject gameObject = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                foreach (EventBinderBehaviour behaviour in
                         gameObject.GetComponentsInChildren<EventBinderBehaviour>(true))
                {
                    if (eventNames.Contains(behaviour.EventName))
                    {
                        continue;
                    }

                    eventNames.Add(behaviour.EventName);

                    builder.AppendLine($"\tpublic struct {behaviour.EventName}");
                    builder.AppendLine("\t{");

                    if (behaviour.Parameters.Count > 0)
                    {
                        foreach ((string type, string name) in behaviour.Parameters)
                        {
                            builder.AppendLine($"\t\tpublic {type} {name};");
                        }
                    }

                    builder.AppendLine("\t}");
                }
            }

            foreach (string key in settings.AllPopup.Keys)
            {
                string path = sourcePath.Replace("/Source/", "/Prefabs/")
                    .Replace("Source.prefab", $"Popup{key}.prefab");
                GameObject gameObject = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                foreach (EventBinderBehaviour behaviour in
                         gameObject.GetComponentsInChildren<EventBinderBehaviour>(true))
                {
                    if (eventNames.Contains(behaviour.EventName))
                    {
                        continue;
                    }

                    eventNames.Add(behaviour.EventName);

                    builder.AppendLine($"\tpublic struct {behaviour.EventName}");
                    builder.AppendLine("\t{");

                    if (behaviour.Parameters.Count > 0)
                    {
                        foreach ((string type, string name) in behaviour.Parameters)
                        {
                            builder.AppendLine($"\t\tpublic {type} {name};");
                        }
                    }

                    builder.AppendLine("\t}");
                }
            }

            CodeSnippetSettings options = CodeSnippetSettings.Load();
            options.Build("CodeSnippet/UIEventType", new Dictionary<string, string>()
                {
                    { "EventType", builder.ToString() }
                }, $"{toolSettings.ComponentGenerateFolder}/{this.WindowName}/UIEventType.cs");
        }
    }
}