using System.Collections.Generic;

namespace Game.Presentation.UI.Windowing
{
    public sealed class WindowFocusController
    {
        private readonly Dictionary<WindowId, int> _zByWindow = new();
        private int _topZ;

        public int Focus(WindowId id)
        {
            _topZ++;
            _zByWindow[id] = _topZ;
            return _topZ;
        }

        public int GetZ(WindowId id) => _zByWindow.TryGetValue(id, out var z) ? z : 0;
    }
}
