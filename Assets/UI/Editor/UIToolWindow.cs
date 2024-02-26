using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace UnityEditor
{
    public sealed class UIToolWindow : OdinMenuEditorWindow
    {
        [MenuItem("Tools/UI")]
        private static void Open()
        {
            UIToolWindow window = GetWindow<UIToolWindow>();
            window.tree = new OdinMenuTree(supportsMultiSelect: false);
            window.tree.AddObjectAtPath("UIBuilder", Resources.Load<UIToolSettings>(nameof(UIToolSettings)));
            window.Show();
        }

        private OdinMenuTree tree;

        protected override OdinMenuTree BuildMenuTree()
        {
            this.tree = new OdinMenuTree(supportsMultiSelect: false);

            {
                UIToolSettings options = UIToolSettings.Load();
                this.tree.AddObjectAtPath("UIBuilder", options);

                foreach (var setting in options.WindowSettings)
                {
                    this.tree.Add(setting.Path, setting);
                }
            }

            {
                CodeSnippetSettings options = CodeSnippetSettings.Load();
                this.tree.AddObjectAtPath("CodeSnippet", options);
                foreach (var template in options.Templates)
                {
                    this.tree.Add(template.Path, template);
                }
            }

            return this.tree;
        }

        public static void Notification(string message, double duration = 0.5)
        {
            UIToolWindow window = GetWindow<UIToolWindow>();
            window.ShowNotification(new GUIContent(message), duration);
        }

        public static void Remove(string path)
        {
            UIToolWindow window = GetWindow<UIToolWindow>();
            window.tree.GetMenuItem(path).Remove();
        }

        public static void Rebuild(string selection = default)
        {
            UIToolWindow window = GetWindow<UIToolWindow>();

            window.ForceMenuTreeRebuild();

            if (!string.IsNullOrEmpty(selection))
            {
                window.tree.GetMenuItem(selection).Select();
            }
        }
    }
}