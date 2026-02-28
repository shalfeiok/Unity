using System;
using System.Collections.Generic;
using Game.Application.Events;
using Game.Presentation.UI.Feedback;
using Game.Presentation.UI.Localization;
using NUnit.Framework;

namespace Game.Tests.EditMode.UI.Feedback
{
    public sealed class UiEventLogServiceTests
    {
        [Test]
        public void Publish_MapsEventToRussianMessage()
        {
            var localizer = new DictionaryLocalizationService(RussianUiStrings.BuildDefault());
            var service = new UiEventLogService(localizer);

            var appEvent = new ApplicationEvent(ApplicationEventType.LootPickedUp, "op_1");
            service.Publish(appEvent);

            Assert.AreEqual(1, service.Entries.Count);
            Assert.AreEqual("Предмет поднят", service.Entries[0].Message);
        }

        [Test]
        public void AddInfo_RespectsMaxEntries()
        {
            var localizer = new DictionaryLocalizationService(RussianUiStrings.BuildDefault());
            var service = new UiEventLogService(localizer, maxEntries: 2);

            service.AddInfo("1");
            service.AddInfo("2");
            service.AddInfo("3");

            Assert.AreEqual(2, service.Entries.Count);
            Assert.AreEqual("2", service.Entries[0].Message);
            Assert.AreEqual("3", service.Entries[1].Message);
        }

        [Test]
        public void AddError_PreservesNonEmptyMessage()
        {
            var localizer = new DictionaryLocalizationService(RussianUiStrings.BuildDefault());
            var service = new UiEventLogService(localizer);

            service.AddError("Ошибка");

            Assert.AreEqual(1, service.Entries.Count);
            Assert.AreEqual("Ошибка", service.Entries[0].Message);
            Assert.LessOrEqual(Math.Abs((DateTime.UtcNow - service.Entries[0].TimestampUtc).TotalSeconds), 3d);
        }

        [Test]
        public void Publish_WithoutLocalization_FallsBackToKey()
        {
            var service = new UiEventLogService(null);
            service.Publish(new ApplicationEvent(ApplicationEventType.CurrencyApplied, "op_2"));

            Assert.AreEqual(1, service.Entries.Count);
            Assert.AreEqual("event.currency_applied", service.Entries[0].Message);
        }

        [Test]
        public void Publish_CurrencyApplied_WithPayload_UsesDetailedMessage()
        {
            var localizer = new DictionaryLocalizationService(RussianUiStrings.BuildDefault());
            var service = new UiEventLogService(localizer);
            var payload = new Dictionary<string, string>
            {
                ["actionId"] = "chaos_orb",
                ["itemId"] = "sword_01"
            };

            service.Publish(new ApplicationEvent(ApplicationEventType.CurrencyApplied, "op_3", payload));

            Assert.AreEqual(1, service.Entries.Count);
            Assert.AreEqual("Валюта chaos_orb применена к предмету sword_01", service.Entries[0].Message);
        }

        [Test]
        public void Publish_LootPickedUp_WithPayload_UsesDetailedMessage()
        {
            var localizer = new DictionaryLocalizationService(RussianUiStrings.BuildDefault());
            var service = new UiEventLogService(localizer);
            var payload = new Dictionary<string, string>
            {
                ["itemId"] = "amulet_rare",
                ["quantity"] = "2",
                ["rarity"] = "Rare"
            };

            service.Publish(new ApplicationEvent(ApplicationEventType.LootPickedUp, "op_4", payload));

            Assert.AreEqual(1, service.Entries.Count);
            Assert.AreEqual("Поднят предмет amulet_rare x2 (Rare)", service.Entries[0].Message);
        }

        [Test]
        public void Publish_GemInserted_WithPayload_UsesDetailedMessage()
        {
            var localizer = new DictionaryLocalizationService(RussianUiStrings.BuildDefault());
            var service = new UiEventLogService(localizer);
            var payload = new Dictionary<string, string>
            {
                ["gemId"] = "support_chain",
                ["socketIndex"] = "3"
            };

            service.Publish(new ApplicationEvent(ApplicationEventType.GemInserted, "op_5", payload));

            Assert.AreEqual(1, service.Entries.Count);
            Assert.AreEqual("Самоцвет support_chain вставлен в сокет #3", service.Entries[0].Message);
        }

        [Test]
        public void Publish_FlaskUsed_WithoutPayload_UsesBaseMessage()
        {
            var localizer = new DictionaryLocalizationService(RussianUiStrings.BuildDefault());
            var service = new UiEventLogService(localizer);

            service.Publish(new ApplicationEvent(ApplicationEventType.FlaskUsed, "op_6"));

            Assert.AreEqual(1, service.Entries.Count);
            Assert.AreEqual("Фласка использована", service.Entries[0].Message);
        }

        [Test]
        public void Publish_FlaskUsed_WithPayload_UsesDetailedMessage()
        {
            var localizer = new DictionaryLocalizationService(RussianUiStrings.BuildDefault());
            var service = new UiEventLogService(localizer);
            var payload = new Dictionary<string, string>
            {
                ["flaskId"] = "life_flask_tier3"
            };

            service.Publish(new ApplicationEvent(ApplicationEventType.FlaskUsed, "op_7", payload));

            Assert.AreEqual(1, service.Entries.Count);
            Assert.AreEqual("Фласка использована: life_flask_tier3", service.Entries[0].Message);
        }

        [Test]
        public void Publish_GemRemoved_WithPayload_UsesDetailedMessage()
        {
            var localizer = new DictionaryLocalizationService(RussianUiStrings.BuildDefault());
            var service = new UiEventLogService(localizer);
            var payload = new Dictionary<string, string>
            {
                ["gemId"] = "support_chain",
                ["socketIndex"] = "3"
            };

            service.Publish(new ApplicationEvent(ApplicationEventType.GemRemoved, "op_8", payload));

            Assert.AreEqual(1, service.Entries.Count);
            Assert.AreEqual("Самоцвет support_chain извлечён из сокета #3", service.Entries[0].Message);
        }

        [Test]
        public void Publish_PassiveAllocated_WithPayload_UsesDetailedMessage()
        {
            var localizer = new DictionaryLocalizationService(RussianUiStrings.BuildDefault());
            var service = new UiEventLogService(localizer);
            var payload = new Dictionary<string, string>
            {
                ["nodeId"] = "notable_fire_mastery"
            };

            service.Publish(new ApplicationEvent(ApplicationEventType.PassiveAllocated, "op_9", payload));

            Assert.AreEqual(1, service.Entries.Count);
            Assert.AreEqual("Выделен пассивный узел: notable_fire_mastery", service.Entries[0].Message);
        }

        [Test]
        public void Publish_PassiveRefunded_WithPayload_UsesDetailedMessage()
        {
            var localizer = new DictionaryLocalizationService(RussianUiStrings.BuildDefault());
            var service = new UiEventLogService(localizer);
            var payload = new Dictionary<string, string>
            {
                ["nodeId"] = "notable_fire_mastery"
            };

            service.Publish(new ApplicationEvent(ApplicationEventType.PassiveRefunded, "op_9_refund", payload));

            Assert.AreEqual(1, service.Entries.Count);
            Assert.AreEqual("Возвращён пассивный узел: notable_fire_mastery", service.Entries[0].Message);
        }

        [Test]
        public void Publish_HotbarAssigned_WithPayload_UsesDetailedMessage()
        {
            var localizer = new DictionaryLocalizationService(RussianUiStrings.BuildDefault());
            var service = new UiEventLogService(localizer);
            var payload = new Dictionary<string, string>
            {
                ["skillId"] = "Fireball",
                ["slot"] = "2"
            };

            service.Publish(new ApplicationEvent(ApplicationEventType.HotbarAssigned, "op_10", payload));

            Assert.AreEqual(1, service.Entries.Count);
            Assert.AreEqual("Навык Fireball назначен в слот 2", service.Entries[0].Message);
        }

        [Test]
        public void Publish_HotbarUnassigned_WithPayload_UsesDetailedMessage()
        {
            var localizer = new DictionaryLocalizationService(RussianUiStrings.BuildDefault());
            var service = new UiEventLogService(localizer);
            var payload = new Dictionary<string, string>
            {
                ["slot"] = "2"
            };

            service.Publish(new ApplicationEvent(ApplicationEventType.HotbarUnassigned, "op_11", payload));

            Assert.AreEqual(1, service.Entries.Count);
            Assert.AreEqual("Навык снят из слота 2", service.Entries[0].Message);
        }

        [Test]
        public void Publish_HotbarAssigned_WithoutSkillId_FallsBackToBaseMessage()
        {
            var localizer = new DictionaryLocalizationService(RussianUiStrings.BuildDefault());
            var service = new UiEventLogService(localizer);
            var payload = new Dictionary<string, string>
            {
                ["slot"] = "2"
            };

            service.Publish(new ApplicationEvent(ApplicationEventType.HotbarAssigned, "op_12", payload));

            Assert.AreEqual(1, service.Entries.Count);
            Assert.AreEqual("Навык назначен на панель", service.Entries[0].Message);
        }
    }
}
