using System.Collections.Generic;
using Game.Application.Poe.UseCases;
using Game.Application.Transactions;
using Game.Domain.Modifiers;
using Game.Domain.Poe.Crafting;
using Game.Domain.Poe.Flasks;
using Game.Domain.Poe.Items;
using Game.Domain.Poe.Passives;
using Game.Domain.Rng;
using Game.Domain.Stats;
using Game.Infrastructure.Economy;
using NUnit.Framework;

namespace Game.Tests.EditMode.App
{
    public sealed class PoeUseCasesTests
    {
        [Test]
        public void ApplyCurrencyActionUseCase_UpdatesItem_AndWritesLedger()
        {
            var runner = new TransactionRunner();
            var engine = new CurrencyActionEngine(new XorShift32Rng(15));
            var ledger = new TransactionLedger();
            var useCase = new ApplyCurrencyActionUseCase(runner, engine, ledger);

            var itemBase = new ItemBaseDefinition { Id = "base_1", ItemClass = "Sword", RequiredItemLevel = 1 };
            var item = new GeneratedPoeItem(itemBase, 80, new List<GeneratedPoeMod>());
            var action = new CurrencyActionDefinition { Id = "add_suffix", Kind = CurrencyActionKind.AddRandomSuffix, Cost = 2 };
            var pool = new List<ModDefinition>
            {
                new() { Id = "s_res", Group = "res", IsPrefix = false, MinItemLevel = 1, MinValue = 10, MaxValue = 20 }
            };

            bool ok = useCase.Execute("op_apply_1", action, item, pool, out var updated);

            Assert.True(ok);
            Assert.AreEqual(1, updated.Mods.Count);
            Assert.AreEqual(1, ledger.Entries.Count);
            Assert.AreEqual("add_suffix", ledger.Entries[0].ActionId);
        }

        [Test]
        public void AllocatePassiveNodeUseCase_AllocatesConnectedNode()
        {
            var tree = new PassiveTreeDefinition();
            tree.Nodes.Add(new PassiveNodeDefinition { Id = "start", IsStart = true, Neighbors = new List<string> { "n1" } });
            tree.Nodes.Add(new PassiveNodeDefinition { Id = "n1", Neighbors = new List<string> { "start" } });

            var allocated = new HashSet<string>();
            var useCase = new AllocatePassiveNodeUseCase(new TransactionRunner(), new PassiveTreeService());

            Assert.True(useCase.Execute("op_alloc_start", tree, allocated, "start", 1));
            Assert.True(useCase.Execute("op_alloc_n1", tree, allocated, "n1", 1));
        }

        [Test]
        public void UseFlaskUseCase_UsesCharges()
        {
            var service = new FlaskService();
            var flask = new FlaskDefinition { Id = "life", MaxCharges = 20, ChargesPerUse = 10, Duration = 4 };
            service.Initialize(flask);

            var useCase = new UseFlaskUseCase(new TransactionRunner(), service);

            Assert.True(useCase.Execute("op_flask_1", flask));
            Assert.AreEqual(10, service.GetCharges(flask.Id));
        }

        [Test]
        public void UseFlaskUseCase_AppliesModifierEffectsToStatSheet()
        {
            var service = new FlaskService();
            var flask = new FlaskDefinition
            {
                Id = "quicksilver",
                MaxCharges = 20,
                ChargesPerUse = 10,
                Duration = 4,
                Effects = { new FlaskEffectDefinition { Stat = StatId.MoveSpeed, Bucket = ModifierBucket.Increased, Value = 0.2f } }
            };
            service.Initialize(flask);

            var stats = new StatSheet();
            stats.SetBase(StatId.MoveSpeed, 100f);

            var useCase = new UseFlaskUseCase(new TransactionRunner(), service);

            Assert.True(useCase.Execute("op_flask_effect_1", flask, stats, sourceId: 9901));
            Assert.AreEqual(120f, stats.GetFinal(StatId.MoveSpeed));
        }
    }
}
