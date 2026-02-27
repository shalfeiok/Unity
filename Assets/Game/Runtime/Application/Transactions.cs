using System;
using System.Collections.Generic;
using Game.Domain;

namespace Game.Application
{
    public interface IUseCase
    {
        string Name { get; }
        void Validate();
        IReadOnlyList<object> Apply();
        IReadOnlyList<object> Publish();
    }

    public interface IEventBus
    {
        void Publish(object domainEvent);
    }

    public interface ILedger
    {
        void Append(LedgerEntry entry);
    }

    public sealed class TransactionRunner
    {
        private readonly FixedTickClock _clock;
        private readonly ILedger _ledger;
        private readonly IEventBus _eventBus;

        public TransactionRunner(FixedTickClock clock, ILedger ledger, IEventBus eventBus)
        {
            _clock = clock;
            _ledger = ledger;
            _eventBus = eventBus;
        }

        public Guid Run(IUseCase useCase)
        {
            useCase.Validate();
            var applied = useCase.Apply();
            var published = useCase.Publish();

            foreach (var domainEvent in applied)
            {
                _eventBus.Publish(domainEvent);
            }

            foreach (var domainEvent in published)
            {
                _eventBus.Publish(domainEvent);
            }

            _clock.Advance();
            var txId = Guid.NewGuid();
            _ledger.Append(new LedgerEntry(txId, useCase.Name, _clock.CurrentTick, DateTime.UtcNow, $"Applied={applied.Count};Published={published.Count}"));
            return txId;
        }
    }
}
