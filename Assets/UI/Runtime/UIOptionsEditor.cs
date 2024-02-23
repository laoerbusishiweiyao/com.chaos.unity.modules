#if UNITY_EDITOR
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.Serialization;

namespace UnityEngine
{
    public sealed partial class UIOptions
    {
        public bool IsSource =>
            this.name.EndsWith("Source") && EditorSceneManager.IsPreviewSceneObject(this.gameObject);

        [BoxGroup("添加", centerLabel: true)]
        [ShowIf(nameof(IsSource))]
        [LabelText("名称")]
        public string InputName;

        [BoxGroup("添加", centerLabel: true)]
        [HorizontalGroup("添加/操作")]
        [ShowIf(nameof(IsSource), true)]
        [Button("弹窗", ButtonSizes.Large)]
        [GUIColor(0.4f, 0.8f, 1)]
        private void AddPopup()
        {
            if (!EditorSceneManager.IsPreviewSceneObject(this.gameObject))
            {
                Debug.LogError("非预制体编辑模式禁止添加弹窗");
                return;
            }

            if (string.IsNullOrEmpty(this.InputName))
            {
                Debug.LogError("弹窗名称不能为空");
                return;
            }

            string path = string.Join('/', Application.dataPath[..^6],
                    PrefabStageUtility.GetPrefabStage(this.gameObject).assetPath)
                .Replace("/Source/", "/Prefabs/")
                .Replace("Source.prefab", $"Popup{this.InputName}.prefab");

            string directory = Path.GetDirectoryName(path);
            if (directory is not null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            GameObject locator = this.PopupParent.gameObject.Child(this.InputName) ??
                                 new GameObject(this.InputName, typeof(RectTransform));
            locator.transform.SetParent(this.PopupParent);
            locator.GetComponent<RectTransform>().SetFullScreen();

            GameObject window = new(name, typeof(RectTransform));
            window.transform.SetParent(locator.transform);
            window.GetComponent<RectTransform>().SetFullScreen();
            window.name = Path.GetFileNameWithoutExtension(path);

            GameObject background = new("BackgroundTrigger", typeof(RectTransform), typeof(UITrigger));
            background.transform.SetParent(window.transform);
            background.GetComponent<RectTransform>().SetFullScreen();

            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(window, path);
            DestroyImmediate(window);
            GameObject popup = PrefabUtility.InstantiatePrefab(prefab, locator.transform) as GameObject;

            Selection.activeObject = popup;

            this.AllPopup[this.InputName] = locator.GetComponent<RectTransform>();

            this.InputName = string.Empty;
        }

        [BoxGroup("添加", centerLabel: true)]
        [HorizontalGroup("添加/操作")]
        [ShowIf(nameof(IsSource), true)]
        [Button("控件", ButtonSizes.Large)]
        [GUIColor(0.4f, 0.8f, 1)]
        private void AddControl()
        {
            if (!EditorSceneManager.IsPreviewSceneObject(this.gameObject))
            {
                Debug.LogError("非预制体编辑模式禁止添加控件");
                return;
            }

            if (PrefabStageUtility.GetCurrentPrefabStage().prefabContentsRoot != this.gameObject)
            {
                Debug.LogError("非根预制体禁止添加控件");
                return;
            }

            if (string.IsNullOrEmpty(this.InputName))
            {
                Debug.LogError("控件名称不能为空");
                return;
            }

            string path = string.Join('/', Application.dataPath[..^6],
                    PrefabStageUtility.GetPrefabStage(this.gameObject).assetPath)
                .Replace("/Source/", "/Prefabs/")
                .Replace("Source.prefab", $"Widget{this.InputName}.prefab");

            string directory = Path.GetDirectoryName(path);
            if (directory is not null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            GameObject locator = this.WidgetParent.gameObject.Child(this.InputName) ??
                                 new GameObject(this.InputName, typeof(RectTransform));
            locator.transform.SetParent(this.WidgetParent);
            locator.GetComponent<RectTransform>().SetFullScreen();

            GameObject widget = new(name, typeof(RectTransform));
            widget.transform.SetParent(locator.transform);
            widget.GetComponent<RectTransform>().SetFullScreen();
            widget.name = Path.GetFileNameWithoutExtension(path);

            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(widget, path);
            DestroyImmediate(widget);
            GameObject popup = PrefabUtility.InstantiatePrefab(prefab, locator.transform) as GameObject;

            Selection.activeObject = popup;

            this.AllWidget[this.InputName] = locator.GetComponent<RectTransform>();

            this.InputName = string.Empty;
        }

        [PropertySpace]
        [ShowIf(nameof(IsSource), true)]
        [Tooltip("生成预制及代码")]
        [Button("生成", ButtonSizes.Gigantic)]
        [GUIColor(0.4f, 0.8f, 1)]
        private void Create()
        {
            if (!EditorSceneManager.IsPreviewSceneObject(this.gameObject))
            {
                Debug.LogError("非预制体编辑模式禁止添加控件");
                return;
            }

            if (PrefabStageUtility.GetCurrentPrefabStage().prefabContentsRoot != this.gameObject)
            {
                Debug.LogError("非根预制体禁止生成");
                return;
            }

            string path = string.Join('/', Application.dataPath[..^6],
                    PrefabStageUtility.GetPrefabStage(this.gameObject).assetPath)
                .Replace("/Source/", "/Prefabs/")
                .Replace("Source.prefab", "Window.prefab");

            string directory = Path.GetDirectoryName(path);
            if (directory is not null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            GameObject window = Instantiate(this.gameObject);
            UIOptions options = window.GetComponent<UIOptions>();

            // 清理控件
            foreach (var widget in options.AllWidget.Values)
            {
                if (options.DefaultLoadedWidgets.Contains(widget))
                {
                    continue;
                }

                widget.CleanImmediate();
            }

            // 清理弹窗
            foreach (var popup in options.AllPopup.Values)
            {
                popup.CleanImmediate();
            }

            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(window, path);
            DestroyImmediate(window);

            Selection.activeObject = prefab;
        }
    }
}
#endif