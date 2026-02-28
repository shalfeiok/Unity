namespace Game.Presentation.UI.Windowing
{
    public sealed class UILayoutAutoSaveHooks
    {
        private readonly UILayoutPersistence _persistence;
        private readonly UILayoutState _state;

        public UILayoutAutoSaveHooks(UILayoutPersistence persistence, UILayoutState state)
        {
            _persistence = persistence;
            _state = state;
        }

        public void OnWindowDragEnded(WindowId windowId) => SaveIfKnown(windowId);
        public void OnWindowResizeEnded(WindowId windowId) => SaveIfKnown(windowId);
        public void OnWindowClosed(WindowId windowId) => SaveIfKnown(windowId);

        private void SaveIfKnown(WindowId windowId)
        {
            for (int i = 0; i < _state.windows.Count; i++)
            {
                if (_state.windows[i].id != windowId)
                    continue;

                _persistence.Save(_state);
                return;
            }
        }
    }
}
