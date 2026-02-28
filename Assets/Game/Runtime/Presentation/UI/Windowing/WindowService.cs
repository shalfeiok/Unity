using System.Collections.Generic;

namespace Game.Presentation.UI.Windowing
{
    public sealed class WindowService : IWindowService
    {
        private readonly WindowRegistry _registry;
        private readonly HashSet<WindowId> _open = new();

        public WindowService(WindowRegistry registry)
        {
            _registry = registry;
        }

        public bool IsOpen(WindowId id) => _open.Contains(id);

        public void Open(WindowId id)
        {
            if (id == WindowId.None || _open.Contains(id)) return;
            if (_registry.TryGet(id, out var setVisible))
            {
                _open.Add(id);
                setVisible(true);
            }
        }

        public void Close(WindowId id)
        {
            if (!_open.Contains(id)) return;
            if (_registry.TryGet(id, out var setVisible))
            {
                _open.Remove(id);
                setVisible(false);
            }
        }

        public void Toggle(WindowId id)
        {
            if (IsOpen(id)) Close(id);
            else Open(id);
        }
    }
}
