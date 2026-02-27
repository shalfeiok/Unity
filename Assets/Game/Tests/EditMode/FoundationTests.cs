using Game.Domain;
using NUnit.Framework;

namespace Game.Tests.EditMode
{
    public class FoundationTests
    {
        [Test]
        public void FixedTickClock_Advance_IncrementsTick()
        {
            var clock = new FixedTickClock(30);
            clock.Advance(2);
            Assert.AreEqual(2, clock.CurrentTick);
        }

        [Test]
        public void RngStreams_AreDeterministicPerSeed()
        {
            var first = new DeterministicRngStreams(123);
            var second = new DeterministicRngStreams(123);

            var a = first.NextInt(RngStreamId.Loot, 0, 1000);
            var b = second.NextInt(RngStreamId.Loot, 0, 1000);
            Assert.AreEqual(a, b);
        }
    }
}
