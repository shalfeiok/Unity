using System;
using System.Collections.Generic;

namespace Game.Application.Events
{
    public readonly struct ApplicationEvent
    {
        public ApplicationEvent(ApplicationEventType type, string operationId, IReadOnlyDictionary<string, string> payload = null)
        {
            Type = type;
            OperationId = operationId ?? string.Empty;
            Payload = payload;
            TimestampUtc = DateTime.UtcNow;
        }

        public ApplicationEventType Type { get; }
        public string OperationId { get; }
        public DateTime TimestampUtc { get; }
        public IReadOnlyDictionary<string, string> Payload { get; }
    }
}
