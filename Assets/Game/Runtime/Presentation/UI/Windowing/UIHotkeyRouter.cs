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
            if (!_bindings.TryGetWindow(hotkey, out var windowId))
                return false;

            _windowManager.Toggle(windowId);
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
