using Game.Application.Events;

namespace Game.Presentation.UI.Feedback
{
    public sealed class ApplicationEventLogSyncService
    {
        private readonly InMemoryApplicationEventPublisher _publisher;
        private readonly UiEventLogService _eventLog;
        private int _lastSyncedIndex;

        public ApplicationEventLogSyncService(InMemoryApplicationEventPublisher publisher, UiEventLogService eventLog)
        {
            _publisher = publisher;
            _eventLog = eventLog;
            _lastSyncedIndex = 0;
        }

        public int SyncNewEvents()
        {
            var events = _publisher.Events;
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
