using System;
using System.Collections.Generic;
using Game.Application.Transactions;
using Game.Domain.Poe.Crafting;
using Game.Domain.Poe.Items;
using Game.Infrastructure.Economy;

namespace Game.Application.Poe.UseCases
{
    public sealed class ApplyCurrencyActionUseCase
    {
        private readonly TransactionRunner _runner;
        private readonly CurrencyActionEngine _engine;
        private readonly TransactionLedger _ledger;

        public ApplyCurrencyActionUseCase(TransactionRunner runner, CurrencyActionEngine engine, TransactionLedger ledger)
        {
            _runner = runner;
            _engine = engine;
            _ledger = ledger;
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
                publish: () => _ledger.Append(operationId, action.Id, currentItem.ItemBase?.Id ?? "unknown", action.Cost));
        }
    }
}
