using System;
using System.Collections.Generic;
using Game.Domain.Rng;

namespace Game.Domain.Loot
{
    public sealed class LootGenerator
    {
        private readonly IRng _rng;

        public LootGenerator(IRng rng)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
        }

        public IReadOnlyList<LootDrop> GenerateDrops(int monsterLevel, int count)
        {
            if (count <= 0) return Array.Empty<LootDrop>();

            var drops = new List<LootDrop>(count);
            for (int i = 0; i < count; i++)
            {
                var rarity = RollRarity(monsterLevel);
                string itemId = RollItemId(rarity);
                int quantity = rarity == LootRarity.Common ? _rng.Range(1, 4) : 1;
                drops.Add(new LootDrop(itemId, rarity, quantity));
            }

            return drops;
        }

        private LootRarity RollRarity(int monsterLevel)
        {
            int tierBonus = monsterLevel / 20;
            float roll = _rng.Next01();

            if (roll < 0.01f + tierBonus * 0.005f) return LootRarity.Legendary;
            if (roll < 0.08f + tierBonus * 0.01f) return LootRarity.Rare;
            if (roll < 0.30f + tierBonus * 0.015f) return LootRarity.Magic;
            return LootRarity.Common;
        }

        private string RollItemId(LootRarity rarity)
        {
            string prefix = rarity switch
            {
                LootRarity.Legendary => "legendary",
                LootRarity.Rare => "rare",
                LootRarity.Magic => "magic",
                _ => "common"
            };

            int suffix = _rng.Range(1000, 9999);
            return $"{prefix}_{suffix}";
        }
    }
}
