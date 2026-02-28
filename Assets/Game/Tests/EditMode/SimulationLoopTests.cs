using Game.Domain.Simulation;
using NUnit.Framework;

namespace Game.Tests.EditMode
{
    public sealed class SimulationLoopTests
    {
        [Test]
        public void Step_ExecutesRegisteredSystemsInOrder()
        {
            var loop = new SimulationLoop(0.02f);
            string trace = string.Empty;

            loop.Register(_ => trace += "A");
            loop.Register(_ => trace += "B");

            int ticks = loop.Step(0.02f);

            Assert.AreEqual(1, ticks);
            Assert.AreEqual("AB", trace);
        }

        [Test]
        public void Step_ConsumesMultipleTicks_WhenFrameDeltaIsLarge()
        {
            var loop = new SimulationLoop(0.02f);
            int executed = 0;
            loop.Register(_ => executed++);

            int ticks = loop.Step(0.1f);

            Assert.AreEqual(5, ticks);
            Assert.AreEqual(5, executed);
            Assert.AreEqual(5, loop.CurrentTime.TickIndex);
        }
    }
}
