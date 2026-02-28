namespace Game.Presentation.UI.Windowing
{
    public interface IWindowService
    {
        bool IsOpen(WindowId id);
        void Open(WindowId id);
        void Close(WindowId id);
        void Toggle(WindowId id);
    }
}
