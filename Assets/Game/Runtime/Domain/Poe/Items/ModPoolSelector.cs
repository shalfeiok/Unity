using System;
using System.Collections.Generic;
using Game.Domain.Rng;

namespace Game.Domain.Poe.Items
{
    public sealed class ModPoolSelector
    {
        private readonly IRng _rng;

        public ModPoolSelector(IRng rng)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
        }

        public IReadOnlyList<ModDefinition> Select(IReadOnlyList<ModDefinition> pool, int itemLevel, int maxPrefixes, int maxSuffixes)
        {
            var eligible = new List<ModDefinition>();
            for (int i = 0; i < pool.Count; i++)
                if (pool[i].MinItemLevel <= itemLevel)
                    eligible.Add(pool[i]);

            var usedGroups = new HashSet<string>();
            var selected = new List<ModDefinition>();
            int prefixes = 0;
            int suffixes = 0;

            while (eligible.Count > 0 && (prefixes < maxPrefixes || suffixes < maxSuffixes))
            {
                int index = _rng.Range(0, eligible.Count);
                var mod = eligible[index];
                eligible.RemoveAt(index);

                if (!string.IsNullOrEmpty(mod.Group) && usedGroups.Contains(mod.Group))
                    continue;

                if (mod.IsPrefix)
                {
                    if (prefixes >= maxPrefixes) continue;
                    prefixes++;
                }
                else
                {
                    if (suffixes >= maxSuffixes) continue;
                    suffixes++;
                }

                if (!string.IsNullOrEmpty(mod.Group))
                    usedGroups.Add(mod.Group);

                selected.Add(mod);
            }

            return selected;
        }
    }
}
