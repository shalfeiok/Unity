using System;

namespace Game.Presentation.UI.Windowing
{
    public sealed class UIHotkeyRouter
    {
        private readonly WindowManager _windowManager;
        private readonly IUIHotkeyBindings _bindings;

        public UIHotkeyRouter(WindowManager windowManager, IUIHotkeyBindings bindings = null)
        {
            _windowManager = windowManager ?? throw new ArgumentNullException(nameof(windowManager));
            _bindings = bindings ?? new DefaultUIHotkeyBindings();
        }

        public bool TryToggle(UIHotkey hotkey)
        {
            return TryToggle(hotkey, out _, out _);
        }

        public bool TryToggle(UIHotkey hotkey, out WindowId windowId, out bool isOpen)
        {
            if (!_bindings.TryGetWindow(hotkey, out windowId))
            {
                isOpen = false;
                return false;
            }

            _windowManager.Toggle(windowId);
            isOpen = _windowManager.IsOpen(windowId);
            return true;
        }

        public bool TryResolveWindow(UIHotkey hotkey, out WindowId windowId)
        {
            return _bindings.TryGetWindow(hotkey, out windowId);
        }

        public bool IsOpen(WindowId windowId)
        {
            return _windowManager.IsOpen(windowId);
        }
    }
}
