using System.Collections.Generic;
using Game.Application;
using Game.Domain;

namespace Game.Infrastructure
{
    public sealed class InMemoryEventBus : IEventBus
    {
        public List<object> Events { get; } = new();

        public void Publish(object domainEvent)
        {
            Events.Add(domainEvent);
        }
    }

    public sealed class InMemoryLedger : ILedger
    {
        public List<LedgerEntry> Entries { get; } = new();

        public void Append(LedgerEntry entry)
        {
            Entries.Add(entry);
        }
    }
}
