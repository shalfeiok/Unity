using System.Collections.Generic;
using Game.Application.Events;
using Game.Application.Transactions;
using Game.Domain.Loot;
using Game.Presentation.UI.Windows.Inventory;

namespace Game.Application.Loot.UseCases
{
    public sealed class PickupLootUseCase
    {
        private readonly TransactionRunner _runner;
        private readonly IApplicationEventPublisher _eventPublisher;

        public PickupLootUseCase(TransactionRunner runner, IApplicationEventPublisher eventPublisher = null)
        {
            _runner = runner;
            _eventPublisher = eventPublisher;
        }

        public bool Execute(string operationId, LootDrop drop, InventoryWindowState inventory)
        {
            bool picked = false;
            _runner.Run(
                operationId,
                validate: () => inventory != null && !string.IsNullOrWhiteSpace(drop.ItemId) && drop.Quantity > 0,
                apply: () =>
                {
                    for (int i = 0; i < drop.Quantity; i++)
                        inventory.ItemIds.Add(drop.ItemId);
                    picked = true;
                },
                publish: () =>
                {
                    if (!picked || _eventPublisher == null)
                        return;

                    _eventPublisher.Publish(new ApplicationEvent(
                        ApplicationEventType.LootPickedUp,
                        operationId,
                        new Dictionary<string, string>
                        {
                            ["itemId"] = drop.ItemId,
                            ["quantity"] = drop.Quantity.ToString(),
                            ["rarity"] = drop.Rarity.ToString()
                        }));
                });

            return picked;
        }
    }
}
