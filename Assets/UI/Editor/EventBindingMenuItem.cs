using UnityEngine.UI;

namespace UnityEditor
{
    public static class EventBindingMenuItem
    {
        [MenuItem("GameObject/EventBinding/PointerClickEventBinder", false, -1)]
        private static void AddTextTMPBinder()
        {
            if (Selection.activeGameObject is null)
            {
                return;
            }

            Selection.activeGameObject.AddComponent<PointerClickEventBinder>();
        }
    }
}