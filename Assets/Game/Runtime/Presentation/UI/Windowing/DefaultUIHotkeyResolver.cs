using System;
using System.Collections.Generic;

namespace Game.Presentation.UI.Windowing
{
    public sealed class DefaultUIHotkeyResolver : IUIHotkeyResolver
    {
        private readonly IReadOnlyDictionary<string, UIHotkey> _map;

        public DefaultUIHotkeyResolver()
            : this(new Dictionary<string, UIHotkey>(StringComparer.OrdinalIgnoreCase)
            {
                ["I"] = UIHotkey.Inventory,
                ["C"] = UIHotkey.Character,
                ["P"] = UIHotkey.PassiveTree,
                ["S"] = UIHotkey.Skills,
                ["K"] = UIHotkey.SkillcraftForge,
                ["O"] = UIHotkey.CraftingBench,
                ["M"] = UIHotkey.Atlas
            })
        {
        }

        public DefaultUIHotkeyResolver(IReadOnlyDictionary<string, UIHotkey> map)
        {
            _map = map;
        }

        public bool TryResolve(string key, out UIHotkey hotkey)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                hotkey = UIHotkey.None;
                return false;
            }

            return _map.TryGetValue(key.Trim(), out hotkey);
        }
    }
}
