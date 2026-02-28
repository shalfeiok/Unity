using System.Collections.Generic;
using Game.Domain.Rng;

namespace Game.Domain.Poe.Items
{
    public sealed class PoeItemGenerator
    {
        private readonly ModPoolSelector _selector;
        private readonly IRng _rng;

        public PoeItemGenerator(IRng rng)
        {
            _rng = rng;
            _selector = new ModPoolSelector(rng);
        }

        public GeneratedPoeItem Generate(ItemBaseDefinition itemBase, IReadOnlyList<ModDefinition> modPool, int itemLevel)
        {
            var selected = _selector.Select(modPool, itemLevel, 3, 3);
            var rolls = new List<GeneratedPoeMod>(selected.Count);
            for (int i = 0; i < selected.Count; i++)
            {
                var mod = selected[i];
                float t = _rng.Next01();
                float value = mod.MinValue + (mod.MaxValue - mod.MinValue) * t;
                rolls.Add(new GeneratedPoeMod(mod, value));
            }

            return new GeneratedPoeItem(itemBase, itemLevel, rolls);
        }
    }

    public readonly struct GeneratedPoeItem
    {
        public GeneratedPoeItem(ItemBaseDefinition itemBase, int itemLevel, IReadOnlyList<GeneratedPoeMod> mods)
        {
            ItemBase = itemBase;
            ItemLevel = itemLevel;
            Mods = mods;
        }

        public ItemBaseDefinition ItemBase { get; }
        public int ItemLevel { get; }
        public IReadOnlyList<GeneratedPoeMod> Mods { get; }
    }

    public readonly struct GeneratedPoeMod
    {
        public GeneratedPoeMod(ModDefinition definition, float rolledValue)
        {
            Definition = definition;
            RolledValue = rolledValue;
        }

        public ModDefinition Definition { get; }
        public float RolledValue { get; }
    }
}
