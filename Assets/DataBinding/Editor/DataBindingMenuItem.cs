using UnityEngine;

namespace UnityEditor
{
    [InitializeOnLoad]
    public static class DataBindingMenuItem
    {
        [MenuItem("GameObject/DataBinding/DataSource", false, -1)]
        private static void AddDataSource()
        {
            if (Selection.activeGameObject is null)
            {
                return;
            }

            Selection.activeGameObject.AddComponent<DataSource>();
        }

        [MenuItem("GameObject/DataBinding/TextTMPBinder", false, 0)]
        private static void AddTextTMPBinder()
        {
            if (Selection.activeGameObject is null)
            {
                return;
            }

            Selection.activeGameObject.AddComponent<TextTMPBinder>();
        }

        [MenuItem("GameObject/DataBinding/TextTMPInputBinder", false, 1)]
        private static void AddTextTMPInputBinder()
        {
            if (Selection.activeGameObject is null)
            {
                return;
            }

            Selection.activeGameObject.AddComponent<TextTMPInputBinder>();
        }

        [MenuItem("GameObject/DataBinding/SliderBinder", false, 2)]
        private static void AddSliderBinder()
        {
            if (Selection.activeGameObject is null)
            {
                return;
            }

            Selection.activeGameObject.AddComponent<SliderBinder>();
        }

        [MenuItem("GameObject/DataBinding/DropdownTMPBinder", false, 3)]
        private static void AddDropdownTMPBinder()
        {
            if (Selection.activeGameObject is null)
            {
                return;
            }

            Selection.activeGameObject.AddComponent<DropdownTMPBinder>();
        }

        [MenuItem("GameObject/DataBinding/DropdownTMPOptionsBinder", false, 4)]
        private static void AddDropdownTMPOptionsBinder()
        {
            if (Selection.activeGameObject is null)
            {
                return;
            }

            Selection.activeGameObject.AddComponent<DropdownTMPOptionsBinder>();
        }

        [MenuItem("GameObject/DataBinding/ToggleGroupBinder", false, 5)]
        private static void AddToggleGroupBinder()
        {
            if (Selection.activeGameObject is null)
            {
                return;
            }

            Selection.activeGameObject.AddComponent<ToggleGroupBinder>();
        }

        [MenuItem("GameObject/DataBinding/GraphicEnableBinder", false, 6)]
        private static void AddGraphicEnableBinder()
        {
            if (Selection.activeGameObject is null)
            {
                return;
            }

            Selection.activeGameObject.AddComponent<GraphicEnableBinder>();
        }

        [MenuItem("GameObject/DataBinding/GraphicEnableCompareBinder", false, 7)]
        private static void AddGraphicEnableCompareBinder()
        {
            if (Selection.activeGameObject is null)
            {
                return;
            }

            Selection.activeGameObject.AddComponent<GraphicEnableCompareBinder>();
        }

        static DataBindingMenuItem()
        {
            DataContextOptions.Default.DataContextTypes.Clear();
            DataContextOptions.Default.DataContextTypes.AddRange(TypeCache.GetTypesDerivedFrom<DataContext>());

            DataContext.Build();
            DataContextCache.Cache();
        }
    }
}