using NUnit.Framework;
using Game.Domain.Rng;

namespace Game.Tests.EditMode
{
    public sealed class RngDeterminismTests
    {
        [Test]
        public void SameSeed_ProducesSameSequence()
        {
            IRng a = new XorShift32Rng(123);
            IRng b = new XorShift32Rng(123);

            for (int i = 0; i < 100; i++)
                Assert.AreEqual(a.NextU32(), b.NextU32());
        }
    }
}
