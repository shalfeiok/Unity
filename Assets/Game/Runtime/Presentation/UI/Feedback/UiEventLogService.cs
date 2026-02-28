using System;
using System.Collections.Generic;
using Game.Application.Events;
using Game.Presentation.UI.Localization;

namespace Game.Presentation.UI.Feedback
{
    public sealed class UiEventLogService
    {
        private readonly ILocalizationService _loc;
        private readonly int _maxEntries;
        private readonly List<UiEventLogEntry> _entries = new();

        public UiEventLogService(ILocalizationService loc, int maxEntries = 32)
        {
            _loc = loc;
            _maxEntries = Math.Max(1, maxEntries);
        }

        public IReadOnlyList<UiEventLogEntry> Entries => _entries;

        public void AddInfo(string message)
        {
            Add(message, DateTime.UtcNow);
        }

        public void AddError(string message)
        {
            Add(message, DateTime.UtcNow);
        }

        public void Publish(ApplicationEvent appEvent)
        {
            var message = appEvent.Type switch
            {
                ApplicationEventType.GemInserted => _loc.Translate("event.gem_inserted"),
                ApplicationEventType.GemRemoved => _loc.Translate("event.gem_removed"),
                ApplicationEventType.PassiveAllocated => _loc.Translate("event.passive_allocated"),
                ApplicationEventType.PassiveRefunded => _loc.Translate("event.passive_refunded"),
                ApplicationEventType.CurrencyApplied => _loc.Translate("event.currency_applied"),
                ApplicationEventType.FlaskUsed => _loc.Translate("event.flask_used"),
                ApplicationEventType.HotbarAssigned => _loc.Translate("event.hotbar_assigned"),
                ApplicationEventType.HotbarUnassigned => _loc.Translate("event.hotbar_unassigned"),
                ApplicationEventType.LootPickedUp => _loc.Translate("event.loot_picked_up"),
                _ => _loc.Translate("event.unknown")
            };

            Add(message, appEvent.TimestampUtc);
        }

        private void Add(string message, DateTime timestampUtc)
        {
            _entries.Add(new UiEventLogEntry(timestampUtc, message ?? string.Empty));
            var overflow = _entries.Count - _maxEntries;
            if (overflow <= 0)
                return;

            _entries.RemoveRange(0, overflow);
        }
    }
}
