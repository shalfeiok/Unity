using System;
using System.Collections.Generic;

namespace Game.Presentation.UI.Windowing
{
    public sealed class DefaultUIHotkeyBindings : IUIHotkeyBindings
    {
        private readonly IReadOnlyDictionary<UIHotkey, WindowId> _map;

        public DefaultUIHotkeyBindings()
            : this(new Dictionary<UIHotkey, WindowId>
            {
                [UIHotkey.Inventory] = WindowId.Inventory,
                [UIHotkey.Character] = WindowId.Character,
                [UIHotkey.PassiveTree] = WindowId.PassiveTree,
                [UIHotkey.Skills] = WindowId.Skills,
                [UIHotkey.SkillcraftForge] = WindowId.Craft,
                [UIHotkey.CraftingBench] = WindowId.Craft,
                [UIHotkey.Atlas] = WindowId.Atlas
            })
        {
        }

        public DefaultUIHotkeyBindings(IReadOnlyDictionary<UIHotkey, WindowId> map)
        {
            _map = map ?? throw new ArgumentNullException(nameof(map));
        }

        public bool TryGetWindow(UIHotkey hotkey, out WindowId windowId)
        {
            return _map.TryGetValue(hotkey, out windowId);
        }
    }
}
