using System.Collections.Generic;
using Game.Domain.Poe.Crafting;
using Game.Domain.Poe.Items;
using Game.Domain.Rng;
using NUnit.Framework;

namespace Game.Tests.EditMode.Poe
{
    public sealed class CurrencyActionEngineTests
    {
        [Test]
        public void AddRandomPrefix_RespectsPrefixCapAndIsDeterministic()
        {
            var pool = BuildPool();
            var baseDef = new ItemBaseDefinition { Id = "base", ItemClass = "Sword", RequiredItemLevel = 1 };
            var item = new GeneratedPoeItem(baseDef, 80, new List<GeneratedPoeMod>
            {
                new(pool[0], 1), new(pool[1], 1), new(pool[2], 1)
            });

            var action = new CurrencyActionDefinition { Id = "exalt_prefix", Kind = CurrencyActionKind.AddRandomPrefix, Cost = 1 };
            var engineA = new CurrencyActionEngine(new XorShift32Rng(123));
            var engineB = new CurrencyActionEngine(new XorShift32Rng(123));

            var a = engineA.Apply(action, item, pool);
            var b = engineB.Apply(action, item, pool);

            Assert.AreEqual(item.Mods.Count, a.Mods.Count); // cap reached already
            Assert.AreEqual(a.Mods.Count, b.Mods.Count);
        }

        [Test]
        public void RemoveRandomMod_RemovesOne_WhenAvailable()
        {
            var pool = BuildPool();
            var baseDef = new ItemBaseDefinition { Id = "base", ItemClass = "Sword", RequiredItemLevel = 1 };
            var item = new GeneratedPoeItem(baseDef, 80, new List<GeneratedPoeMod>
            {
                new(pool[0], 1), new(pool[3], 1)
            });

            var action = new CurrencyActionDefinition { Id = "annul", Kind = CurrencyActionKind.RemoveRandomMod, Cost = 1 };
            var result = new CurrencyActionEngine(new XorShift32Rng(7)).Apply(action, item, pool);

            Assert.AreEqual(1, result.Mods.Count);
        }

        private static List<ModDefinition> BuildPool()
        {
            return new List<ModDefinition>
            {
                new() { Id = "p1", Group = "g1", IsPrefix = true, MinItemLevel = 1, MinValue = 1, MaxValue = 2 },
                new() { Id = "p2", Group = "g2", IsPrefix = true, MinItemLevel = 1, MinValue = 1, MaxValue = 2 },
                new() { Id = "p3", Group = "g3", IsPrefix = true, MinItemLevel = 1, MinValue = 1, MaxValue = 2 },
                new() { Id = "s1", Group = "s1", IsPrefix = false, MinItemLevel = 1, MinValue = 1, MaxValue = 2 },
                new() { Id = "s2", Group = "s2", IsPrefix = false, MinItemLevel = 1, MinValue = 1, MaxValue = 2 }
            };
        }
    }
}
