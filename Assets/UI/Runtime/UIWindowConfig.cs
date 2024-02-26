using System.Collections.Generic;

namespace UnityEngine
{
    public sealed class UIWindowConfig
    {
        public string Name;

        public UILayer Layer = UILayer.Window;

        public int Priority;

        public string Asset;

        public Dictionary<string, UIWidgetConfig> Controls = new();
    }
}