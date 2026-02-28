using System;

namespace Game.Presentation.UI.Windowing
{
    public sealed class UIInputRouter
    {
        private readonly InputContextStack _contextStack;
        private readonly UIHotkeyRouter _hotkeyRouter;
        private readonly IUIHotkeyResolver _hotkeyResolver;
        private readonly IUIBackNavigation _backNavigation;

        public UIInputRouter(
            InputContextStack contextStack,
            UIHotkeyRouter hotkeyRouter,
            IUIHotkeyResolver hotkeyResolver = null,
            IUIBackNavigation backNavigation = null)
        {
            _contextStack = contextStack ?? throw new ArgumentNullException(nameof(contextStack));
            _hotkeyRouter = hotkeyRouter ?? throw new ArgumentNullException(nameof(hotkeyRouter));
            _hotkeyResolver = hotkeyResolver ?? new DefaultUIHotkeyResolver();
            _backNavigation = backNavigation ?? NullUIBackNavigation.Instance;
        }

        public bool TryHandleToggleKey(string key)
        {
            if (_contextStack.Current == InputContext.Modal)
                return false;

            if (!_hotkeyResolver.TryResolve(key, out var hotkey))
                return false;

            return _hotkeyRouter.TryToggle(hotkey);
        }

        public bool TryHandleEscape()
        {
            if (_backNavigation.HasModal())
                return _backNavigation.TryCloseModal();

            if (_contextStack.Current == InputContext.UI || _contextStack.Current == InputContext.Modal)
                return _backNavigation.TryCloseTopPanel();

            return false;
        }
    }
}
