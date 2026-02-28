using System.Collections.Generic;
using Game.Domain.Combat;
using Game.Domain.Modifiers;

namespace Game.Domain.Stats
{
    public sealed class StatSheet
    {
        private readonly Dictionary<StatId, float> _base = new();
        private readonly ModifierSet _modifiers = new();

        public void SetBase(StatId stat, float value) => _base[stat] = value;

        public float GetBase(StatId stat) => _base.TryGetValue(stat, out var v) ? v : 0f;

        public void AddModifier(in Modifier modifier) => _modifiers.Add(modifier);

        public void RemoveAllFromSource(int sourceId) => _modifiers.RemoveAllFromSource(sourceId);

        public float GetFinal(StatId stat) => _modifiers.Compute(stat, GetBase(stat)).FinalValue;

        public DamageBreakdown GetDamageBreakdown(StatId stat)
        {
            return GetDamageBreakdown(stat, critChance: 0f, critMultiplier: 1.5f, mitigation: 0f);
        }

        public DamageBreakdown GetDamageBreakdown(StatId stat, float critChance, float critMultiplier, float mitigation)
        {
            var baseValue = GetBase(stat);
            var computation = _modifiers.Compute(stat, baseValue);
            return new DamageBreakdown(computation, baseValue, critChance, critMultiplier, mitigation);
        }

        public ModifierComputation GetComputation(StatId stat) => _modifiers.Compute(stat, GetBase(stat));
    }
}
