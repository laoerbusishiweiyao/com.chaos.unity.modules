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

        [MenuItem("GameObject/DataBinding/TextTMPBinder", false, -1)]
        private static void AddTextTMPBinder()
        {
            if (Selection.activeGameObject is null)
            {
                return;
            }

            Selection.activeGameObject.AddComponent<TextTMPBinder>();
        }

        [MenuItem("GameObject/DataBinding/SliderBinder", false, -1)]
        private static void AddSliderBinder()
        {
            if (Selection.activeGameObject is null)
            {
                return;
            }

            Selection.activeGameObject.AddComponent<SliderBinder>();
        }
        
        [MenuItem("GameObject/DataBinding/DropdownTMPBinder", false, -1)]
        private static void AddDropdownTMPBinder()
        {
            if (Selection.activeGameObject is null)
            {
                return;
            }

            Selection.activeGameObject.AddComponent<DropdownTMPBinder>();
        }
        
        [MenuItem("GameObject/DataBinding/DropdownTMPOptionsBinder", false, -1)]
        private static void AddDropdownTMPOptionsBinder()
        {
            if (Selection.activeGameObject is null)
            {
                return;
            }

            Selection.activeGameObject.AddComponent<DropdownTMPOptionsBinder>();
        }
        
        [MenuItem("GameObject/DataBinding/GraphicEnableBinder", false, -1)]
        private static void AddGraphicEnableBinder()
        {
            if (Selection.activeGameObject is null)
            {
                return;
            }

            Selection.activeGameObject.AddComponent<GraphicEnableBinder>();
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