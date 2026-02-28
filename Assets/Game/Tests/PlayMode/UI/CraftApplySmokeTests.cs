using System.Collections.Generic;
using Game.Application.Poe.UseCases;
using Game.Application.Transactions;
using Game.Domain.Poe.Crafting;
using Game.Domain.Poe.Items;
using Game.Domain.Rng;
using Game.Infrastructure.Economy;
using NUnit.Framework;

namespace Game.Tests.PlayMode.UI
{
    public sealed class CraftApplySmokeTests
    {
        [Test]
        public void ApplyCurrencyAction_ThroughUseCase_Smoke()
        {
            var useCase = new ApplyCurrencyActionUseCase(
                new TransactionRunner(),
                new CurrencyActionEngine(new XorShift32Rng(321)),
                new TransactionLedger());

            var itemBase = new ItemBaseDefinition { Id = "base_sword", ItemClass = "Sword", RequiredItemLevel = 1 };
            var item = new GeneratedPoeItem(itemBase, 80, new List<GeneratedPoeMod>());
            var action = new CurrencyActionDefinition { Id = "pm_add_suffix", Kind = CurrencyActionKind.AddRandomSuffix, Cost = 1 };
            var pool = new List<ModDefinition>
            {
                new() { Id = "s_res_fire", Group = "res_fire", IsPrefix = false, MinItemLevel = 1, MinValue = 10, MaxValue = 20 }
            };

            bool ok = useCase.Execute("pm_craft_1", action, item, pool, out var updated);

            Assert.True(ok);
            Assert.AreEqual(1, updated.Mods.Count);
        }
    }
}
