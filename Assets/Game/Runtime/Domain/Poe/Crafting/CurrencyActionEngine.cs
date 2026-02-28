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
            var socketColors = new List<SocketColor>(item.SocketColors);
            var linkGroups = CloneLinks(item.LinkGroups);
            int quality = item.Quality;
            bool isCorrupted = item.IsCorrupted;

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

                case CurrencyActionKind.ReforgeWithBias:
                    mods.Clear();
                    FillWithRandomMods(mods, BuildBiasedPool(modPool, action.BiasTag), item.ItemLevel, 4);
                    break;

                case CurrencyActionKind.SetSocketCount:
                    int targetSockets = Clamp(action.IntValue, 0, item.ItemBase?.MaxSockets ?? 6);
                    socketColors = BuildSockets(targetSockets);
                    linkGroups = BuildLinks(targetSockets, linkChance: 0.35f);
                    break;

                case CurrencyActionKind.RerollSocketColors:
                    for (int i = 0; i < socketColors.Count; i++)
                        socketColors[i] = RollSocketColor();
                    break;

                case CurrencyActionKind.RerollLinks:
                    linkGroups = BuildLinks(socketColors.Count, linkChance: 0.45f);
                    break;

                case CurrencyActionKind.ImproveQuality:
                    quality = Clamp(quality + action.IntValue, 0, 20);
                    break;

                case CurrencyActionKind.Corrupt:
                    isCorrupted = true;
                    break;
            }

            return new GeneratedPoeItem(item.ItemBase, item.ItemLevel, mods, item.Implicits, quality, socketColors, linkGroups, isCorrupted);
        }

        private void FillWithRandomMods(List<GeneratedPoeMod> mods, IReadOnlyList<ModDefinition> pool, int itemLevel, int targetCount)
        {
            for (int i = 0; i < targetCount; i++)
                TryAddRandom(mods, pool, itemLevel, wantPrefix: i % 2 == 0);
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

        private IReadOnlyList<ModDefinition> BuildBiasedPool(IReadOnlyList<ModDefinition> pool, string biasTag)
        {
            if (string.IsNullOrWhiteSpace(biasTag))
                return pool;

            var weighted = new List<ModDefinition>(pool.Count * 2);
            for (int i = 0; i < pool.Count; i++)
            {
                var mod = pool[i];
                weighted.Add(mod);
                if ((mod.Group != null && mod.Group.IndexOf(biasTag, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (mod.Id != null && mod.Id.IndexOf(biasTag, StringComparison.OrdinalIgnoreCase) >= 0))
                    weighted.Add(mod);
            }

            return weighted;
        }

        private List<SocketColor> BuildSockets(int count)
        {
            var result = new List<SocketColor>(count);
            for (int i = 0; i < count; i++)
                result.Add(RollSocketColor());
            return result;
        }

        private SocketColor RollSocketColor()
        {
            int roll = _craftRng.Range(0, 100);
            if (roll < 33) return SocketColor.Red;
            if (roll < 66) return SocketColor.Green;
            return SocketColor.Blue;
        }

        private List<int[]> BuildLinks(int socketCount, float linkChance)
        {
            var result = new List<int[]>();
            if (socketCount <= 0) return result;

            var current = new List<int> { 0 };
            for (int i = 1; i < socketCount; i++)
            {
                if (_craftRng.Next01() <= linkChance)
                {
                    current.Add(i);
                }
                else
                {
                    result.Add(current.ToArray());
                    current = new List<int> { i };
                }
            }

            result.Add(current.ToArray());
            return result;
        }

        private static List<int[]> CloneLinks(IReadOnlyList<int[]> source)
        {
            var result = new List<int[]>(source.Count);
            for (int i = 0; i < source.Count; i++)
            {
                var group = source[i];
                var clone = new int[group.Length];
                Array.Copy(group, clone, group.Length);
                result.Add(clone);
            }

            return result;
        }

        private static int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
    }
}
