using Game.Application.Events;
using System;

namespace Game.Presentation.UI.Feedback
{
    public sealed class ApplicationEventLogSyncService
    {
        private readonly IApplicationEventReader _eventReader;
        private readonly UiEventLogService _eventLog;
        private int _lastSyncedIndex;

        public ApplicationEventLogSyncService(IApplicationEventReader eventReader, UiEventLogService eventLog)
        {
            _eventReader = eventReader ?? throw new ArgumentNullException(nameof(eventReader));
            _eventLog = eventLog ?? throw new ArgumentNullException(nameof(eventLog));
            _lastSyncedIndex = 0;
        }

        public int SyncNewEvents()
        {
            var events = _eventReader.Events;
            if (_lastSyncedIndex >= events.Count)
                return 0;

            int synced = 0;
            for (int i = _lastSyncedIndex; i < events.Count; i++)
            {
                _eventLog.Publish(events[i]);
                synced++;
            }

            _lastSyncedIndex = events.Count;
            return synced;
        }
    }
}
