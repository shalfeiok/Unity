using Game.Presentation.UI.Windowing;
using NUnit.Framework;

namespace Game.Tests.EditMode
{
    public sealed class UiRefreshSchedulerTests
    {
        [Test]
        public void ExecuteBudgeted_RespectsPerFrameLimit()
        {
            var scheduler = new UIRefreshScheduler();
            int calls = 0;

            scheduler.Enqueue(() => calls++);
            scheduler.Enqueue(() => calls++);
            scheduler.Enqueue(() => calls++);

            int executed = scheduler.ExecuteBudgeted(2);

            Assert.AreEqual(2, executed);
            Assert.AreEqual(2, calls);
            Assert.AreEqual(1, scheduler.PendingCount);
        }
    }
}
