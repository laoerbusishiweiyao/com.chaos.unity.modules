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

        static DataBindingMenuItem()
        {
            DataContextOptions.Default.DataContextTypes.Clear();
            DataContextOptions.Default.DataContextTypes.AddRange(TypeCache.GetTypesDerivedFrom<DataContext>());
            
            DataContext.Build();
            DataContextCache.Cache();
        }
    }
}