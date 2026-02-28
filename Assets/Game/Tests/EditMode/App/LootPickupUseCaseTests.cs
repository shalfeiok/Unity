using Game.Application.Events;
using Game.Application.Loot.UseCases;
using Game.Application.Transactions;
using Game.Domain.Loot;
using Game.Presentation.UI.Windows.Inventory;
using NUnit.Framework;

namespace Game.Tests.EditMode.App
{
    public sealed class LootPickupUseCaseTests
    {
        [Test]
        public void Execute_AddsItemsToInventory_PublishesEvent_AndIsIdempotent()
        {
            var publisher = new InMemoryApplicationEventPublisher();
            var useCase = new PickupLootUseCase(new TransactionRunner(), publisher);
            var inventory = new InventoryWindowState();
            var drop = new LootDrop("rare_sword", LootRarity.Rare, 2);

            Assert.True(useCase.Execute("op_pickup_1", drop, inventory));
            Assert.True(useCase.Execute("op_pickup_1", drop, inventory));

            Assert.AreEqual(2, inventory.ItemIds.Count);
            Assert.AreEqual(1, publisher.Events.Count);
            Assert.AreEqual(ApplicationEventType.LootPickedUp, publisher.Events[0].Type);
        }
    }
}
