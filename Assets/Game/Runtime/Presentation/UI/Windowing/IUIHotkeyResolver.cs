namespace Game.Presentation.UI.Windowing
{
    public interface IUIHotkeyResolver
    {
        bool TryResolve(string key, out UIHotkey hotkey);
    }
}
