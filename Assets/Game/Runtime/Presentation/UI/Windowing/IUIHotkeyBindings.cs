namespace Game.Presentation.UI.Windowing
{
    public interface IUIHotkeyBindings
    {
        bool TryGetWindow(UIHotkey hotkey, out WindowId windowId);
    }
}
