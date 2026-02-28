using System.Collections.Generic;
using Game.Application.Poe.UseCases;
using Game.Application.Transactions;
using Game.Domain.Poe.Crafting;
using Game.Domain.Poe.Items;
using Game.Domain.Rng;
using Game.Infrastructure.Economy;
using Game.Presentation.UI.Windows.Craft;
using NUnit.Framework;

namespace Game.Tests.EditMode.UI.Windows
{
    public sealed class CraftWindowServiceTests
    {
        [Test]
        public void Apply_UpdatesStateCurrentMods_AndClearsPreview()
        {
            var runner = new TransactionRunner();
            var useCase = new ApplyCurrencyActionUseCase(
                runner,
                new CurrencyActionEngine(new XorShift32Rng(123)),
                new TransactionLedger());
            var service = new CraftWindowService(useCase);
            var state = new CraftWindowState();

            var itemBase = new ItemBaseDefinition { Id = "base_1", ItemClass = "Sword", RequiredItemLevel = 1 };
            var current = new GeneratedPoeItem(itemBase, 80, new List<GeneratedPoeMod>());
            var action = new CurrencyActionDefinition { Id = "add_suffix", Kind = CurrencyActionKind.AddRandomSuffix, Cost = 1 };
            var pool = new List<ModDefinition>
            {
                new() { Id = "s_res", Group = "res", IsPrefix = false, MinItemLevel = 1, MinValue = 1, MaxValue = 2 }
            };

            service.BuildPreview(state, current);
            Assert.True(state.HasPreview);

            bool ok = service.Apply(state, "op_craft_1", action, current, pool, out var updated);

            Assert.True(ok);
            Assert.AreEqual(1, updated.Mods.Count);
            Assert.AreEqual(1, state.CurrentModIds.Count);
            Assert.False(state.HasPreview);
            Assert.AreEqual(0, state.PreviewModIds.Count);
        }
    }
}
