using System;
using System.Collections.Generic;
using Game.Application.Abstractions;

namespace Game.Infrastructure.Economy
{
    public sealed class TransactionLedger : ITransactionLedger
    {
        private readonly List<LedgerEntry> _entries = new();

        public IReadOnlyList<LedgerEntry> Entries => _entries;

        public void Append(string operationId, string actionId, string itemId, int currencyCost)
        {
            _entries.Add(new LedgerEntry(operationId, actionId, itemId, currencyCost, DateTime.UtcNow));
        }
    }

    public readonly struct LedgerEntry
    {
        public LedgerEntry(string operationId, string actionId, string itemId, int currencyCost, DateTime timestampUtc)
        {
            OperationId = operationId;
            ActionId = actionId;
            ItemId = itemId;
            CurrencyCost = currencyCost;
            TimestampUtc = timestampUtc;
        }

        public string OperationId { get; }
        public string ActionId { get; }
        public string ItemId { get; }
        public int CurrencyCost { get; }
        public DateTime TimestampUtc { get; }
    }
}
