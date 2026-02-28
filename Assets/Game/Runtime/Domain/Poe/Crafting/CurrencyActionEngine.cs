using System;
using System.Collections.Generic;
using Game.Domain.Poe.Items;
using Game.Domain.Rng;

namespace Game.Domain.Poe.Crafting
{
    public sealed class CurrencyActionEngine
    {
        private readonly IRng _craftRng;

        public CurrencyActionEngine(IRng craftRng)
        {
            _craftRng = craftRng ?? throw new ArgumentNullException(nameof(craftRng));
        }

        public GeneratedPoeItem Apply(
            CurrencyActionDefinition action,
            GeneratedPoeItem item,
            IReadOnlyList<ModDefinition> modPool)
        {
            var mods = new List<GeneratedPoeMod>(item.Mods);

            switch (action.Kind)
            {
                case CurrencyActionKind.RerollAllMods:
                    mods.Clear();
                    FillWithRandomMods(mods, modPool, item.ItemLevel, 4);
                    break;

                case CurrencyActionKind.AddRandomPrefix:
                    TryAddRandom(mods, modPool, item.ItemLevel, wantPrefix: true);
                    break;

                case CurrencyActionKind.AddRandomSuffix:
                    TryAddRandom(mods, modPool, item.ItemLevel, wantPrefix: false);
                    break;

                case CurrencyActionKind.RemoveRandomMod:
                    if (mods.Count > 0)
                    {
                        int index = _craftRng.Range(0, mods.Count);
                        mods.RemoveAt(index);
                    }
                    break;
            }

            return new GeneratedPoeItem(item.ItemBase, item.ItemLevel, mods);
        }

        private void FillWithRandomMods(List<GeneratedPoeMod> mods, IReadOnlyList<ModDefinition> pool, int itemLevel, int targetCount)
        {
            for (int i = 0; i < targetCount; i++)
            {
                TryAddRandom(mods, pool, itemLevel, wantPrefix: i % 2 == 0);
            }
        }

        private void TryAddRandom(List<GeneratedPoeMod> mods, IReadOnlyList<ModDefinition> pool, int itemLevel, bool wantPrefix)
        {
            int prefixes = 0;
            int suffixes = 0;
            var usedGroups = new HashSet<string>();

            for (int i = 0; i < mods.Count; i++)
            {
                if (mods[i].Definition.IsPrefix) prefixes++;
                else suffixes++;
                if (!string.IsNullOrEmpty(mods[i].Definition.Group))
                    usedGroups.Add(mods[i].Definition.Group);
            }

            if (wantPrefix && prefixes >= 3) return;
            if (!wantPrefix && suffixes >= 3) return;

            var candidates = new List<ModDefinition>();
            for (int i = 0; i < pool.Count; i++)
            {
                var mod = pool[i];
                if (mod.IsPrefix != wantPrefix) continue;
                if (mod.MinItemLevel > itemLevel) continue;
                if (!string.IsNullOrEmpty(mod.Group) && usedGroups.Contains(mod.Group)) continue;
                candidates.Add(mod);
            }

            if (candidates.Count == 0) return;

            var pick = candidates[_craftRng.Range(0, candidates.Count)];
            float value = pick.MinValue + (pick.MaxValue - pick.MinValue) * _craftRng.Next01();
            mods.Add(new GeneratedPoeMod(pick, value));
        }
    }
}
