namespace Game.Presentation.UI.Windowing
{
    public interface IUIBackNavigation
    {
        bool HasModal();
        bool TryCloseModal();
        bool TryCloseTopPanel();
    }
}
