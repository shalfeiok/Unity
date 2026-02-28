using System.Collections.Generic;
using Game.Application.Abstractions;
using Game.Application.Events;
using Game.Application.Transactions;
using Game.Domain.Poe.Crafting;
using Game.Domain.Poe.Items;

namespace Game.Application.Poe.UseCases
{
    public sealed class ApplyCurrencyActionUseCase
    {
        private readonly TransactionRunner _runner;
        private readonly CurrencyActionEngine _engine;
        private readonly ITransactionLedger _ledger;
        private readonly IApplicationEventPublisher _eventPublisher;

        public ApplyCurrencyActionUseCase(
            TransactionRunner runner,
            CurrencyActionEngine engine,
            ITransactionLedger ledger,
            IApplicationEventPublisher eventPublisher = null)
        {
            _runner = runner;
            _engine = engine;
            _ledger = ledger;
            _eventPublisher = eventPublisher;
        }

        public bool Execute(
            string operationId,
            CurrencyActionDefinition action,
            GeneratedPoeItem currentItem,
            IReadOnlyList<ModDefinition> modPool,
            out GeneratedPoeItem updatedItem)
        {
            updatedItem = currentItem;

            return _runner.Run(
                operationId,
                validate: () => action != null && !string.IsNullOrEmpty(action.Id),
                apply: () => updatedItem = _engine.Apply(action, currentItem, modPool),
                publish: () =>
                {
                    _ledger.Append(operationId, action.Id, currentItem.ItemBase?.Id ?? "unknown", action.Cost);
                    _eventPublisher?.Publish(new ApplicationEvent(
                        ApplicationEventType.CurrencyApplied,
                        operationId,
                        new Dictionary<string, string>
                        {
                            ["actionId"] = action.Id,
                            ["itemId"] = currentItem.ItemBase?.Id ?? "unknown"
                        }));
                });
        }
    }
}
