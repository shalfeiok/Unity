using System;
using System.Collections.Generic;

namespace Game.Presentation.UI.Windowing
{
    [Serializable]
    public sealed class UILayoutState
    {
        public List<WindowState> windows = new();

        [Serializable]
        public sealed class WindowState
        {
            public WindowId id;
            public float x;
            public float y;
            public float width;
            public float height;
            public string dock;
            public bool collapsed;
            public bool pinned;
            public int zIndex;
            public bool active;
            public int activeTab;
            public string customJson;
        }
    }
}
