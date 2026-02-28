namespace Game.Presentation.UI.Windowing
{
    public sealed class NullUIBackNavigation : IUIBackNavigation
    {
        public static readonly NullUIBackNavigation Instance = new();

        private NullUIBackNavigation()
        {
        }

        public bool HasModal() => false;

        public bool TryCloseModal() => false;

        public bool TryCloseTopPanel() => false;

        public void NotifyWindowState(WindowId windowId, bool isOpen)
        {
        }

        public void EnterModal()
        {
        }

        public void ExitModal()
        {
        }
    }
}
