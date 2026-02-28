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

            var implicits = new List<GeneratedPoeMod>();
            if (itemBase?.ImplicitMods != null)
            {
                for (int i = 0; i < itemBase.ImplicitMods.Count; i++)
                {
                    var mod = itemBase.ImplicitMods[i];
                    float t = _rng.Next01();
                    float value = mod.MinValue + (mod.MaxValue - mod.MinValue) * t;
                    implicits.Add(new GeneratedPoeMod(mod, value));
                }
            }

            return new GeneratedPoeItem(itemBase, itemLevel, rolls, implicits, quality: 0, socketColors: System.Array.Empty<SocketColor>(), linkGroups: System.Array.Empty<int[]>(), isCorrupted: false);
        }
    }

    public readonly struct GeneratedPoeItem
    {
        public GeneratedPoeItem(ItemBaseDefinition itemBase, int itemLevel, IReadOnlyList<GeneratedPoeMod> mods)
            : this(itemBase, itemLevel, mods, System.Array.Empty<GeneratedPoeMod>(), 0, System.Array.Empty<SocketColor>(), System.Array.Empty<int[]>(), false)
        {
        }

        public GeneratedPoeItem(
            ItemBaseDefinition itemBase,
            int itemLevel,
            IReadOnlyList<GeneratedPoeMod> mods,
            IReadOnlyList<GeneratedPoeMod> implicits,
            int quality,
            IReadOnlyList<SocketColor> socketColors,
            IReadOnlyList<int[]> linkGroups,
            bool isCorrupted)
        {
            ItemBase = itemBase;
            ItemLevel = itemLevel;
            Mods = mods;
            Implicits = implicits;
            Quality = quality;
            SocketColors = socketColors;
            LinkGroups = linkGroups;
            IsCorrupted = isCorrupted;
        }

        public ItemBaseDefinition ItemBase { get; }
        public int ItemLevel { get; }
        public IReadOnlyList<GeneratedPoeMod> Mods { get; }
        public IReadOnlyList<GeneratedPoeMod> Implicits { get; }
        public int Quality { get; }
        public IReadOnlyList<SocketColor> SocketColors { get; }
        public IReadOnlyList<int[]> LinkGroups { get; }
        public bool IsCorrupted { get; }
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
