namespace Game.Presentation.UI.Windowing
{
    public sealed class WindowManager
    {
        private readonly IWindowService _windowService;

        public WindowManager(IWindowService windowService)
        {
            _windowService = windowService;
        }

        public void Show(WindowId windowId) => _windowService.Open(windowId);
        public void Hide(WindowId windowId) => _windowService.Close(windowId);
        public void Toggle(WindowId windowId) => _windowService.Toggle(windowId);
        public bool IsOpen(WindowId windowId) => _windowService.IsOpen(windowId);
    }
}
