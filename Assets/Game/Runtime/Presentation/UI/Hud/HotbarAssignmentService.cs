using System;
using System.Collections.Generic;

namespace Game.Presentation.UI.Hud
{
    public sealed class HotbarAssignmentService
    {
        private readonly Dictionary<int, string> _bindings = new();

        public bool Assign(int slotIndex, string skillId)
        {
            if (slotIndex < 0 || slotIndex > 9) return false;
            if (string.IsNullOrWhiteSpace(skillId)) return false;

            _bindings[slotIndex] = skillId;
            return true;
        }

        public bool Unassign(int slotIndex)
        {
            return _bindings.Remove(slotIndex);
        }

        public bool TryGetAssignedSkill(int slotIndex, out string skillId)
        {
            return _bindings.TryGetValue(slotIndex, out skillId);
        }

        public IReadOnlyDictionary<int, string> Snapshot() => _bindings;

        public void Clear() => _bindings.Clear();
    }
}
