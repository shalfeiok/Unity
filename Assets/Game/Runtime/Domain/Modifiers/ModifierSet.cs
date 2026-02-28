using System.Collections.Generic;
using Game.Domain.Stats;

namespace Game.Domain.Modifiers
{
    public sealed class ModifierSet
    {
        private readonly List<Modifier> _mods = new();

        public void Add(in Modifier modifier) => _mods.Add(modifier);

        public void RemoveAllFromSource(int sourceId)
        {
            for (int i = _mods.Count - 1; i >= 0; i--)
                if (_mods[i].SourceId == sourceId) _mods.RemoveAt(i);
        }

        public ModifierComputation Compute(StatId stat, float baseValue)
        {
            float add = 0f;
            float increased = 0f;
            float more = 1f;
            float cap = float.PositiveInfinity;
            var conversions = new List<Modifier>();

            for (int i = 0; i < _mods.Count; i++)
            {
                ref readonly var mod = ref _mods[i];
                if (mod.Stat != stat) continue;

                switch (mod.Bucket)
                {
                    case ModifierBucket.Base:
                    case ModifierBucket.Add:
                        add += mod.Value;
                        break;
                    case ModifierBucket.Increased:
                        increased += mod.Value;
                        break;
                    case ModifierBucket.More:
                        more *= 1f + mod.Value;
                        break;
                    case ModifierBucket.Cap:
                        if (mod.Value < cap) cap = mod.Value;
                        break;
                    case ModifierBucket.Conversion:
                        conversions.Add(mod);
                        break;
                }
            }

            conversions.Sort(static (a, b) => a.SourceId.CompareTo(b.SourceId));
            float valueBeforeConversion = (baseValue + add) * (1f + increased) * more;
            float remaining = valueBeforeConversion;
            float converted = 0f;

            for (int i = 0; i < conversions.Count; i++)
            {
                float ratio = conversions[i].Value;
                if (ratio < 0f) ratio = 0f;
                if (ratio > 1f) ratio = 1f;

                float piece = remaining * ratio;
                remaining -= piece;
                converted += piece;
            }

            float finalValue = remaining;
            if (!float.IsPositiveInfinity(cap) && finalValue > cap)
                finalValue = cap;

            return new ModifierComputation(finalValue, valueBeforeConversion, add, increased, more, converted, conversions);
        }
    }

    public readonly struct ModifierComputation
    {
        public ModifierComputation(
            float finalValue,
            float valueBeforeConversion,
            float add,
            float increased,
            float more,
            float convertedOut,
            IReadOnlyList<Modifier> orderedConversions)
        {
            FinalValue = finalValue;
            ValueBeforeConversion = valueBeforeConversion;
            Add = add;
            Increased = increased;
            More = more;
            ConvertedOut = convertedOut;
            OrderedConversions = orderedConversions;
        }

        public float FinalValue { get; }
        public float ValueBeforeConversion { get; }
        public float Add { get; }
        public float Increased { get; }
        public float More { get; }
        public float ConvertedOut { get; }
        public IReadOnlyList<Modifier> OrderedConversions { get; }
    }
}
