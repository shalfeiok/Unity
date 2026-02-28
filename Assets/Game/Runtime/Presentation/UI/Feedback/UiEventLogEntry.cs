using System;

namespace Game.Presentation.UI.Feedback
{
    public readonly struct UiEventLogEntry
    {
        public UiEventLogEntry(DateTime timestampUtc, string message)
        {
            TimestampUtc = timestampUtc;
            Message = message ?? string.Empty;
        }

        public DateTime TimestampUtc { get; }
        public string Message { get; }
    }
}
