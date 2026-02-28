using System;
using System.Collections.Generic;

namespace Game.Presentation.UI.Windowing
{
    public sealed class WindowStackBackNavigation : IUIBackNavigation
    {
        private readonly IWindowService _windowService;
        private readonly List<WindowId> _stack = new();
        private int _modalDepth;

        public WindowStackBackNavigation(IWindowService windowService)
        {
            _windowService = windowService ?? throw new ArgumentNullException(nameof(windowService));
        }

        public bool HasModal() => _modalDepth > 0;

        public bool TryCloseModal()
        {
            if (_modalDepth <= 0)
                return false;

            _modalDepth--;
            return true;
        }

        public bool TryCloseTopPanel()
        {
            for (int i = _stack.Count - 1; i >= 0; i--)
            {
                var windowId = _stack[i];
                if (_windowService.IsOpen(windowId))
                {
                    _windowService.Close(windowId);
                    _stack.RemoveAt(i);
                    return true;
                }

                _stack.RemoveAt(i);
            }

            return false;
        }

        public void NotifyWindowState(WindowId windowId, bool isOpen)
        {
            if (windowId == WindowId.None)
                return;

            int idx = _stack.IndexOf(windowId);
            if (isOpen)
            {
                if (idx >= 0)
                    _stack.RemoveAt(idx);

                _stack.Add(windowId);
                return;
            }

            if (idx >= 0)
                _stack.RemoveAt(idx);
        }

        public void PushModal() => _modalDepth++;
    }
}
