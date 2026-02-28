using System.Collections.Generic;

namespace Game.Presentation.UI.Windowing
{
    public sealed class UIHotkeyRouter
    {
        private static readonly IReadOnlyDictionary<UIHotkey, WindowId> _toggleMap =
            new Dictionary<UIHotkey, WindowId>
            {
                [UIHotkey.Inventory] = WindowId.Inventory,
                [UIHotkey.Character] = WindowId.Character,
                [UIHotkey.PassiveTree] = WindowId.PassiveTree,
                [UIHotkey.Skills] = WindowId.Skills,
                [UIHotkey.SkillcraftForge] = WindowId.Craft,
                [UIHotkey.CraftingBench] = WindowId.Craft,
                [UIHotkey.Atlas] = WindowId.Atlas
            };

        private readonly WindowManager _windowManager;

        public UIHotkeyRouter(WindowManager windowManager)
        {
            _windowManager = windowManager;
        }

        public bool TryToggle(UIHotkey hotkey)
        {
            if (!_toggleMap.TryGetValue(hotkey, out var windowId))
                return false;

            _windowManager.Toggle(windowId);
            return true;
        }

        public bool TryResolveWindow(UIHotkey hotkey, out WindowId windowId)
        {
            return _toggleMap.TryGetValue(hotkey, out windowId);
        }
    }
}
