using System;
using System.IO;
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

            // 加载控件

            // 加载弹窗

            PrefabUtility.SaveAsPrefabAsset(window, path);
            UnityEngine.Object.DestroyImmediate(window);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void BuildWindow()
        {
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
    }
}