using System.Collections.Generic;
using Game.Application.Events;
using Game.Application.Poe.UseCases;
using Game.Application.Transactions;
using Game.Application.UI.UseCases;
using Game.Domain.Poe.Crafting;
using Game.Domain.Poe.Items;
using Game.Domain.Poe.Passives;
using Game.Domain.Poe.Sockets;
using Game.Domain.Rng;
using Game.Infrastructure.Economy;
using Game.Presentation.UI.Hud;
using NUnit.Framework;

namespace Game.Tests.EditMode.App
{
    public sealed class ApplicationEventPublicationTests
    {
        [Test]
        public void InsertGemUseCase_PublishesGemInsertedEvent()
        {
            var publisher = new InMemoryApplicationEventPublisher();
            var useCase = new InsertGemUseCase(new TransactionRunner(), new SocketService(), publisher);
            var sockets = new SocketModel(2);

            Assert.True(useCase.Execute("op_event_insert", sockets, 0, "fireball"));
            Assert.AreEqual(1, publisher.Events.Count);
            Assert.AreEqual(ApplicationEventType.GemInserted, publisher.Events[0].Type);
        }

        [Test]
        public void CurrencyUseCase_PublishesCurrencyAppliedEvent()
        {
            var publisher = new InMemoryApplicationEventPublisher();
            var ledger = new TransactionLedger();
            var useCase = new ApplyCurrencyActionUseCase(
                new TransactionRunner(),
                new CurrencyActionEngine(new XorShift32Rng(12)),
                ledger,
                publisher);

            var item = new GeneratedPoeItem(
                new ItemBaseDefinition { Id = "base" },
                10,
                new List<GeneratedPoeMod>());
            var action = new CurrencyActionDefinition { Id = "add_suffix", Kind = CurrencyActionKind.AddRandomSuffix, Cost = 1 };
            var pool = new List<ModDefinition>
            {
                new() { Id = "m1", Group = "grp", IsPrefix = false, MinValue = 1, MaxValue = 2, MinItemLevel = 1 }
            };

            Assert.True(useCase.Execute("op_event_currency", action, item, pool, out _));
            Assert.AreEqual(1, publisher.Events.Count);
            Assert.AreEqual(ApplicationEventType.CurrencyApplied, publisher.Events[0].Type);
        }

        [Test]
        public void HotbarUseCase_PublishesAssignAndUnassignEvents()
        {
            var publisher = new InMemoryApplicationEventPublisher();
            var useCase = new AssignSkillToHotbarUseCase(new TransactionRunner(), new HotbarAssignmentService(), publisher);

            Assert.True(useCase.Execute("op_event_hotbar_assign", 1, "Arc"));
            Assert.True(useCase.Unassign("op_event_hotbar_unassign", 1));

            Assert.AreEqual(2, publisher.Events.Count);
            Assert.AreEqual(ApplicationEventType.HotbarAssigned, publisher.Events[0].Type);
            Assert.AreEqual(ApplicationEventType.HotbarUnassigned, publisher.Events[1].Type);
        }

        [Test]
        public void PassiveUseCase_PublishesOnlyOnSuccess()
        {
            var publisher = new InMemoryApplicationEventPublisher();
            var useCase = new AllocatePassiveNodeUseCase(new TransactionRunner(), new PassiveTreeService(), publisher);

            var tree = new PassiveTreeDefinition();
            tree.Nodes.Add(new PassiveNodeDefinition { Id = "start", IsStart = true, Neighbors = new List<string>() });
            var allocated = new HashSet<string>();

            Assert.True(useCase.Execute("op_event_alloc", tree, allocated, "start", 1));
            Assert.False(useCase.Execute("op_event_alloc_fail", tree, allocated, "missing", 1));

            Assert.AreEqual(1, publisher.Events.Count);
            Assert.AreEqual(ApplicationEventType.PassiveAllocated, publisher.Events[0].Type);
        }
    }
}
