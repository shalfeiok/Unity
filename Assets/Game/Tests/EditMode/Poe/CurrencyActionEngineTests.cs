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

            Assert.AreEqual(item.Mods.Count, a.Mods.Count);
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

        [Test]
        public void ReforgeWithBias_PrefersBiasedGroups_Deterministically()
        {
            var pool = BuildPool();
            var item = new GeneratedPoeItem(new ItemBaseDefinition { Id = "wand", ItemClass = "Wand" }, 80, new List<GeneratedPoeMod>());
            var action = new CurrencyActionDefinition { Id = "bias_fire", Kind = CurrencyActionKind.ReforgeWithBias, BiasTag = "fire" };

            var result = new CurrencyActionEngine(new XorShift32Rng(11)).Apply(action, item, pool);

            Assert.GreaterOrEqual(result.Mods.Count, 1);
            bool hasFire = false;
            for (int i = 0; i < result.Mods.Count; i++)
            {
                if (result.Mods[i].Definition.Group.Contains("fire"))
                    hasFire = true;
            }

            Assert.True(hasFire);
        }

        [Test]
        public void SocketAndQualityActions_UpdateItemState()
        {
            var pool = BuildPool();
            var itemBase = new ItemBaseDefinition { Id = "armor", ItemClass = "Armor", MaxSockets = 6 };
            var item = new GeneratedPoeItem(itemBase, 70, new List<GeneratedPoeMod>());
            var engine = new CurrencyActionEngine(new XorShift32Rng(19));

            var setSockets = engine.Apply(new CurrencyActionDefinition { Kind = CurrencyActionKind.SetSocketCount, IntValue = 4 }, item, pool);
            var rerollColors = engine.Apply(new CurrencyActionDefinition { Kind = CurrencyActionKind.RerollSocketColors }, setSockets, pool);
            var rerollLinks = engine.Apply(new CurrencyActionDefinition { Kind = CurrencyActionKind.RerollLinks }, rerollColors, pool);
            var quality = engine.Apply(new CurrencyActionDefinition { Kind = CurrencyActionKind.ImproveQuality, IntValue = 8 }, rerollLinks, pool);
            var corrupted = engine.Apply(new CurrencyActionDefinition { Kind = CurrencyActionKind.Corrupt }, quality, pool);

            Assert.AreEqual(4, setSockets.SocketColors.Count);
            Assert.AreEqual(4, rerollColors.SocketColors.Count);
            Assert.GreaterOrEqual(rerollLinks.LinkGroups.Count, 1);
            Assert.AreEqual(8, quality.Quality);
            Assert.True(corrupted.IsCorrupted);
        }

        private static List<ModDefinition> BuildPool()
        {
            return new List<ModDefinition>
            {
                new() { Id = "p1_fire", Group = "g1_fire", IsPrefix = true, MinItemLevel = 1, MinValue = 1, MaxValue = 2 },
                new() { Id = "p2", Group = "g2", IsPrefix = true, MinItemLevel = 1, MinValue = 1, MaxValue = 2 },
                new() { Id = "p3", Group = "g3", IsPrefix = true, MinItemLevel = 1, MinValue = 1, MaxValue = 2 },
                new() { Id = "s1_fire", Group = "s1_fire", IsPrefix = false, MinItemLevel = 1, MinValue = 1, MaxValue = 2 },
                new() { Id = "s2", Group = "s2", IsPrefix = false, MinItemLevel = 1, MinValue = 1, MaxValue = 2 }
            };
        }
    }
}
