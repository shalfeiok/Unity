using System;
using System.Collections.Generic;

namespace Game.Presentation.UI.Windowing
{
    public sealed class WindowRegistry
    {
        private readonly Dictionary<WindowId, Action<bool>> _entries = new();

        public void Register(WindowId id, Action<bool> setVisible)
        {
            if (setVisible == null) throw new ArgumentNullException(nameof(setVisible));
            _entries[id] = setVisible;
        }

        public bool TryGet(WindowId id, out Action<bool> setVisible) => _entries.TryGetValue(id, out setVisible);
    }
}
