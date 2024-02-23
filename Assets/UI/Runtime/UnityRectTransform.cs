namespace UnityEngine
{
    public static class UnityRectTransform
    {
        public static void SetFullScreen(this RectTransform self)
        {
            self.anchorMin = Vector2.zero;
            self.anchorMax = Vector2.one;
            self.anchoredPosition3D = Vector3.zero;
            self.pivot = Vector2.one * 0.5f;
            self.offsetMax = Vector2.zero;
            self.offsetMin = Vector2.zero;
            self.sizeDelta = Vector2.zero;
            self.localEulerAngles = Vector3.zero;
            self.localScale = Vector3.one;
        }
    }

    public static class UnityTransform
    {
        public static Transform Child(this Transform self, string path)
        {
            return self.Find(path);
        }

        public static void Clean(this Transform self)
        {
            int length = self.childCount;
            if (length == 0)
            {
                return;
            }

            for (int i = 0; i < length; i++)
            {
                Object.Destroy(self.GetChild(0).gameObject);
            }
        }

        public static void CleanImmediate(this Transform self)
        {
            int length = self.childCount;
            if (length == 0)
            {
                return;
            }

            for (int i = 0; i < length; i++)
            {
                Object.DestroyImmediate(self.GetChild(0).gameObject);
            }
        }
    }

    public static class UnityGameObject
    {
        public static GameObject Child(this GameObject self, string name)
        {
            return self.transform.Child(name)?.gameObject;
        }
    }
}