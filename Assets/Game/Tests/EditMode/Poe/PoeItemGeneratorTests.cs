using System.Collections.Generic;
using Game.Domain.Poe.Items;
using Game.Domain.Rng;
using NUnit.Framework;

namespace Game.Tests.EditMode.Poe
{
    public sealed class PoeItemGeneratorTests
    {
        [Test]
        public void Generate_RespectsPrefixSuffixCaps_AndIsDeterministic()
        {
            var pool = BuildPool();

            var g1 = new PoeItemGenerator(new XorShift32Rng(42));
            var g2 = new PoeItemGenerator(new XorShift32Rng(42));
            var itemBase = new ItemBaseDefinition { Id = "sword_1", ItemClass = "Sword", RequiredItemLevel = 1 };

            var a = g1.Generate(itemBase, pool, 80);
            var b = g2.Generate(itemBase, pool, 80);

            int prefixes = 0;
            int suffixes = 0;
            for (int i = 0; i < a.Mods.Count; i++)
            {
                if (a.Mods[i].Definition.IsPrefix) prefixes++;
                else suffixes++;

                Assert.AreEqual(a.Mods[i].Definition.Id, b.Mods[i].Definition.Id);
                Assert.AreEqual(a.Mods[i].RolledValue, b.Mods[i].RolledValue);
            }

            Assert.LessOrEqual(prefixes, 3);
            Assert.LessOrEqual(suffixes, 3);
        }

        [Test]
        public void Generate_RespectsItemLevelGating()
        {
            var pool = BuildPool();
            var generator = new PoeItemGenerator(new XorShift32Rng(5));
            var itemBase = new ItemBaseDefinition { Id = "wand_1", ItemClass = "Wand", RequiredItemLevel = 1 };

            var lowIlvl = generator.Generate(itemBase, pool, 1);
            for (int i = 0; i < lowIlvl.Mods.Count; i++)
                Assert.LessOrEqual(lowIlvl.Mods[i].Definition.MinItemLevel, 1);
        }

        [Test]
        public void Generate_RollsImplicits_FromBaseDefinition()
        {
            var pool = BuildPool();
            var baseDef = new ItemBaseDefinition
            {
                Id = "axe", ItemClass = "Axe", RequiredItemLevel = 1,
                ImplicitMods = new List<ModDefinition>
                {
                    new() { Id = "implicit_phys", Group = "imp_phys", IsPrefix = true, MinItemLevel = 1, MinValue = 5, MaxValue = 10 }
                }
            };
            var item = new PoeItemGenerator(new XorShift32Rng(3)).Generate(baseDef, pool, 20);

            Assert.AreEqual(1, item.Implicits.Count);
            Assert.AreEqual("implicit_phys", item.Implicits[0].Definition.Id);
        }

        private static List<ModDefinition> BuildPool()
        {
            return new List<ModDefinition>
            {
                new() { Id = "p_life_t3", Group = "life", IsPrefix = true, Tier = 3, MinItemLevel = 1, MinValue = 10, MaxValue = 20 },
                new() { Id = "p_life_t1", Group = "life", IsPrefix = true, Tier = 1, MinItemLevel = 70, MinValue = 80, MaxValue = 100 },
                new() { Id = "p_phys_t2", Group = "phys", IsPrefix = true, Tier = 2, MinItemLevel = 20, MinValue = 30, MaxValue = 45 },
                new() { Id = "s_res_fire_t2", Group = "res_fire", IsPrefix = false, Tier = 2, MinItemLevel = 10, MinValue = 15, MaxValue = 25 },
                new() { Id = "s_res_cold_t1", Group = "res_cold", IsPrefix = false, Tier = 1, MinItemLevel = 60, MinValue = 30, MaxValue = 40 },
                new() { Id = "s_dex_t3", Group = "dex", IsPrefix = false, Tier = 3, MinItemLevel = 1, MinValue = 12, MaxValue = 25 }
            };
        }
    }
}
