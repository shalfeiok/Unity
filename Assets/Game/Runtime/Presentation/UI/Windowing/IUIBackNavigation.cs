namespace Game.Presentation.UI.Windowing
{
    public interface IUIBackNavigation
    {
        bool HasModal();
        bool TryCloseModal();
        bool TryCloseTopPanel();
        void NotifyWindowState(WindowId windowId, bool isOpen);
    }
}
