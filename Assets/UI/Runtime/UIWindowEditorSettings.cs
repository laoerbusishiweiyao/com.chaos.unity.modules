#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace UnityEngine
{
    [AddComponentMenu("")]
    public sealed class UIWindowEditorSettings : SerializedMonoBehaviour
    {
        [LabelText("名称")]
        [ReadOnly]
        public string WindowName;

        [BoxGroup("设置", centerLabel: true)]
        [LabelText("层级")]
        public UILayer Layer = UILayer.Window;

        [BoxGroup("设置", centerLabel: true)]
        [LabelText("优先级")]
        public int Priority;

        [BoxGroup("节点数据", centerLabel: true)]
        [LabelText("默认加载控件集合")]
        [Tooltip("拖拽赋值")]
        [ListDrawerSettings(HideAddButton = true, DraggableItems = false)]
        public List<RectTransform> DefaultLoadedWidgets = new();

        [BoxGroup("节点数据", centerLabel: true)]
        [BoxGroup("节点数据/控件", centerLabel: true)]
        [LabelText("所有控件父节点")]
        [ReadOnly]
        public RectTransform WidgetParent;

        [BoxGroup("节点数据", centerLabel: true)]
        [BoxGroup("节点数据/控件", centerLabel: true)]
        [LabelText("控件集合")]
        [DictionaryDrawerSettings(
            KeyLabel = "名称",
            ValueLabel = "定位点",
            IsReadOnly = true,
            DisplayMode = DictionaryDisplayOptions.OneLine)]
        [ReadOnly]
        public Dictionary<string, RectTransform> AllWidget = new();

        [BoxGroup("节点数据", centerLabel: true)]
        [BoxGroup("节点数据/弹窗", centerLabel: true)]
        [LabelText("所有弹窗父节点")]
        [ReadOnly]
        public RectTransform PopupParent;

        [BoxGroup("节点数据", centerLabel: true)]
        [BoxGroup("节点数据/弹窗", centerLabel: true)]
        [LabelText("弹窗集合")]
        [DictionaryDrawerSettings(
            KeyLabel = "名称",
            ValueLabel = "定位点",
            IsReadOnly = true,
            DisplayMode = DictionaryDisplayOptions.OneLine)]
        [ReadOnly]
        public Dictionary<string, RectTransform> AllPopup = new();

        #region Editor

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

            int layer = LayerMask.NameToLayer("UI");
            GameObject locator = this.PopupParent.gameObject.Child(this.InputName) ??
                                 new GameObject(this.InputName, typeof(RectTransform));
            locator.transform.SetParent(this.PopupParent);
            locator.GetComponent<RectTransform>().SetFullScreen();
            locator.layer = layer;

            GameObject widget = new(name, typeof(RectTransform), typeof(UIWidgetEditorSettings));
            widget.transform.SetParent(locator.transform);
            widget.GetComponent<RectTransform>().SetFullScreen();
            widget.name = Path.GetFileNameWithoutExtension(path);
            widget.AddComponent(System.Type.GetType("UnityEngine.DataSource"));

            GameObject background = new("BackgroundTrigger", typeof(RectTransform), typeof(UITrigger));
            background.transform.SetParent(widget.transform);
            background.GetComponent<RectTransform>().SetFullScreen();

            UIWidgetEditorSettings options = widget.GetComponent<UIWidgetEditorSettings>();
            options.WidgetName = this.InputName;
            options.Type = UIWidgetType.Popup;

            foreach (Transform child in widget.GetComponentsInChildren<Transform>())
            {
                transform.gameObject.layer = layer;
            }

            widget.layer = layer;

            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(widget, path);
            DestroyImmediate(widget);

            PrefabUtility.InstantiatePrefab(prefab, locator.transform);

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

            int layer = LayerMask.NameToLayer("UI");

            GameObject locator = this.WidgetParent.gameObject.Child(this.InputName) ??
                                 new GameObject(this.InputName, typeof(RectTransform));
            locator.transform.SetParent(this.WidgetParent);
            locator.GetComponent<RectTransform>().SetFullScreen();
            locator.layer = layer;

            GameObject widget = new(name, typeof(RectTransform), typeof(UIWidgetEditorSettings));
            widget.transform.SetParent(locator.transform);
            widget.GetComponent<RectTransform>().SetFullScreen();
            widget.name = Path.GetFileNameWithoutExtension(path);
            widget.AddComponent(System.Type.GetType("UnityEngine.DataSource"));

            UIWidgetEditorSettings options = widget.GetComponent<UIWidgetEditorSettings>();
            options.WidgetName = this.InputName;
            options.Type = UIWidgetType.Default;

            foreach (Transform child in widget.GetComponentsInChildren<Transform>())
            {
                transform.gameObject.layer = layer;
            }

            widget.layer = layer;

            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(widget, path);
            DestroyImmediate(widget);

            PrefabUtility.InstantiatePrefab(prefab, locator.transform);

            this.AllWidget[this.InputName] = locator.GetComponent<RectTransform>();

            this.InputName = string.Empty;
        }

        [Button("清理", ButtonSizes.Large)]
        [GUIColor(0.4f, 0.8f, 1)]
        public void Clean()
        {
            var stage = PrefabStageUtility.GetCurrentPrefabStage();
            if (stage is not null)
            {
                Debug.LogError("请先退出预制体编辑模式");
                return;
            }
            
            var defaultLoadedWidgets = this.DefaultLoadedWidgets;
            this.DefaultLoadedWidgets.Clear();
            foreach (RectTransform widget in defaultLoadedWidgets)
            {
                if (widget is null)
                {
                    continue;
                }

                this.DefaultLoadedWidgets.Add(widget);
            }

            foreach (string key in this.AllWidget.Where(pair => pair.Value is null).Select(pair => pair.Key).ToArray())
            {
                this.AllWidget.Remove(key);
            }

            foreach (string key in this.AllPopup.Where(pair => pair.Value is null).Select(pair => pair.Key).ToArray())
            {
                this.AllPopup.Remove(key);
            }
        }

        #endregion
    }
}
#endif