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
                ApplicationEventType.GemInserted => BuildGemInsertedMessage(appEvent),
                ApplicationEventType.GemRemoved => BuildGemRemovedMessage(appEvent),
                ApplicationEventType.PassiveAllocated => Translate("event.passive_allocated"),
                ApplicationEventType.PassiveRefunded => Translate("event.passive_refunded"),
                ApplicationEventType.CurrencyApplied => BuildCurrencyAppliedMessage(appEvent),
                ApplicationEventType.FlaskUsed => BuildFlaskUsedMessage(appEvent),
                ApplicationEventType.HotbarAssigned => Translate("event.hotbar_assigned"),
                ApplicationEventType.HotbarUnassigned => Translate("event.hotbar_unassigned"),
                ApplicationEventType.LootPickedUp => BuildLootPickedUpMessage(appEvent),
                _ => Translate("event.unknown")
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

        private string Translate(string key)
        {
            return _loc == null ? key : _loc.Translate(key);
        }

        private string TranslateFormat(string key, params object[] args)
        {
            return _loc == null ? key : _loc.TranslateFormat(key, args);
        }

        private string BuildCurrencyAppliedMessage(ApplicationEvent appEvent)
        {
            if (!TryReadPayloadValue(appEvent, "actionId", out var actionId) ||
                !TryReadPayloadValue(appEvent, "itemId", out var itemId))
            {
                return Translate("event.currency_applied");
            }

            return TranslateFormat("event.currency_applied_detailed", actionId, itemId);
        }

        private string BuildGemInsertedMessage(ApplicationEvent appEvent)
        {
            if (!TryReadPayloadValue(appEvent, "gemId", out var gemId) ||
                !TryReadPayloadValue(appEvent, "socketIndex", out var socketIndex))
            {
                return Translate("event.gem_inserted");
            }

            return TranslateFormat("event.gem_inserted_detailed", gemId, socketIndex);
        }

        private string BuildGemRemovedMessage(ApplicationEvent appEvent)
        {
            if (!TryReadPayloadValue(appEvent, "gemId", out var gemId) ||
                !TryReadPayloadValue(appEvent, "socketIndex", out var socketIndex))
            {
                return Translate("event.gem_removed");
            }

            return TranslateFormat("event.gem_removed_detailed", gemId, socketIndex);
        }

        private string BuildFlaskUsedMessage(ApplicationEvent appEvent)
        {
            if (!TryReadPayloadValue(appEvent, "flaskId", out var flaskId))
                return Translate("event.flask_used");

            return TranslateFormat("event.flask_used_detailed", flaskId);
        }

        private string BuildLootPickedUpMessage(ApplicationEvent appEvent)
        {
            if (!TryReadPayloadValue(appEvent, "itemId", out var itemId) ||
                !TryReadPayloadValue(appEvent, "quantity", out var quantity) ||
                !TryReadPayloadValue(appEvent, "rarity", out var rarity))
            {
                return Translate("event.loot_picked_up");
            }

            return TranslateFormat("event.loot_picked_up_detailed", itemId, quantity, rarity);
        }

        private static bool TryReadPayloadValue(ApplicationEvent appEvent, string key, out string value)
        {
            value = string.Empty;
            if (appEvent.Payload == null)
                return false;

            if (!appEvent.Payload.TryGetValue(key, out value))
                return false;

            return !string.IsNullOrWhiteSpace(value);
        }
    }
}
