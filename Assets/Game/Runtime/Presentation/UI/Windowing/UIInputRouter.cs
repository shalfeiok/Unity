namespace Game.Presentation.UI.Windowing
{
    public sealed class UIInputRouter
    {
        private readonly InputContextStack _contextStack;
        private readonly UIHotkeyRouter _hotkeyRouter;
        private readonly IUIHotkeyResolver _hotkeyResolver;

        public UIInputRouter(
            InputContextStack contextStack,
            UIHotkeyRouter hotkeyRouter,
            IUIHotkeyResolver hotkeyResolver = null)
        {
            _contextStack = contextStack;
            _hotkeyRouter = hotkeyRouter;
            _hotkeyResolver = hotkeyResolver ?? new DefaultUIHotkeyResolver();
        }

        public bool TryHandleToggleKey(string key)
        {
            if (_contextStack.Current == InputContext.Modal)
                return false;

            if (!_hotkeyResolver.TryResolve(key, out var hotkey))
                return false;

            return _hotkeyRouter.TryToggle(hotkey);
        }
    }
}
