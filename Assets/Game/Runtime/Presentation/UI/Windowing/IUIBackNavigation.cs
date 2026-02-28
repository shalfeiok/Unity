namespace Game.Presentation.UI.Windowing
{
    public interface IUIBackNavigation
    {
        bool HasModal();
        bool TryCloseModal();
        bool TryCloseTopPanel();
        bool HasOpenPanels();
        void NotifyWindowState(WindowId windowId, bool isOpen);
        void EnterModal();
        void ExitModal();
    }
}
