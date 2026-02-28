using System;
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
    }
}
