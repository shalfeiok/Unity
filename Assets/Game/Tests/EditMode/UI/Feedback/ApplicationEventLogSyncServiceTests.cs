using Game.Application.Events;
using Game.Presentation.UI.Feedback;
using Game.Presentation.UI.Localization;
using NUnit.Framework;
using System;

namespace Game.Tests.EditMode.UI.Feedback
{
    public sealed class ApplicationEventLogSyncServiceTests
    {
        [Test]
        public void Ctor_NullDependencies_Throws()
        {
            var localizer = new DictionaryLocalizationService(RussianUiStrings.BuildDefault());
            var eventLog = new UiEventLogService(localizer);
            var publisher = new InMemoryApplicationEventPublisher();

            Assert.Throws<ArgumentNullException>(() => new ApplicationEventLogSyncService(null, eventLog));
            Assert.Throws<ArgumentNullException>(() => new ApplicationEventLogSyncService(publisher, null));
        }

        [Test]
        public void SyncNewEvents_PublishesOnlyDelta()
        {
            var publisher = new InMemoryApplicationEventPublisher();
            var localizer = new DictionaryLocalizationService(RussianUiStrings.BuildDefault());
            var eventLog = new UiEventLogService(localizer);
            var sync = new ApplicationEventLogSyncService(publisher, eventLog);

            publisher.Publish(new ApplicationEvent(ApplicationEventType.GemInserted, "op_1"));
            publisher.Publish(new ApplicationEvent(ApplicationEventType.LootPickedUp, "op_2"));

            var first = sync.SyncNewEvents();
            var second = sync.SyncNewEvents();

            Assert.AreEqual(2, first);
            Assert.AreEqual(0, second);
            Assert.AreEqual(2, eventLog.Entries.Count);
            Assert.AreEqual("Самоцвет вставлен", eventLog.Entries[0].Message);
            Assert.AreEqual("Предмет поднят", eventLog.Entries[1].Message);
        }
    }
}
