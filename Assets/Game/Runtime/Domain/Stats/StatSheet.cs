using System;
using System.Collections.Generic;

namespace Game.Domain.Stats
{
    /// <summary>
    /// Minimal stat sheet skeleton. Expand with soft caps, breakdown and pooling.
    /// Keep allocation-free in hot paths.
    /// </summary>
    public sealed class StatSheet
    {
        private readonly Dictionary<StatId, float> _base = new();
        private readonly List<Modifier> _mods = new();

        public void SetBase(StatId stat, float value) => _base[stat] = value;

        public float GetBase(StatId stat) => _base.TryGetValue(stat, out var v) ? v : 0f;

        public void AddModifier(Modifier m) => _mods.Add(m);

        public void RemoveAllFromSource(int sourceId)
        {
            for (int i = _mods.Count - 1; i >= 0; i--)
                if (_mods[i].SourceId == sourceId) _mods.RemoveAt(i);
        }

        public float GetFinal(StatId stat)
        {
            float value = GetBase(stat);

            float addFlat = 0f;
            float addPct = 0f;
            float mul = 1f;
            bool hasOverride = false;
            float overrideValue = 0f;

            for (int i = 0; i < _mods.Count; i++)
            {
                ref readonly var m = ref _mods[i];
                if (m.Stat != stat) continue;

                switch (m.Op)
                {
                    case ModifierOp.AddFlat: addFlat += m.Value; break;
                    case ModifierOp.AddPercent: addPct += m.Value; break;
                    case ModifierOp.Multiply: mul *= (1f + m.Value); break;
                    case ModifierOp.Override:
                        hasOverride = true;
                        overrideValue = m.Value; // TODO: add priority if needed
                        break;
                }
            }

            value += addFlat;
            value *= (1f + addPct);
            value *= mul;
            if (hasOverride) value = overrideValue;

            return value;
        }
    }
}
