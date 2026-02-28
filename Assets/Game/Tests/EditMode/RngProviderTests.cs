using Game.Domain.Rng;
using NUnit.Framework;

namespace Game.Tests.EditMode
{
    public sealed class RngProviderTests
    {
        [Test]
        public void SameSeed_ProducesSamePerStreamSequences_For100Iterations()
        {
            var providerA = new RngProvider(777);
            var providerB = new RngProvider(777);

            for (int i = 0; i < 100; i++)
            {
                Assert.AreEqual(
                    providerA.GetStream(RngStreamId.Combat).NextU32(),
                    providerB.GetStream(RngStreamId.Combat).NextU32());

                Assert.AreEqual(
                    providerA.GetStream(RngStreamId.Loot).NextU32(),
                    providerB.GetStream(RngStreamId.Loot).NextU32());
            }
        }

        [Test]
        public void DifferentStreams_AreIndependent()
        {
            var provider = new RngProvider(777);

            var combat = provider.GetStream(RngStreamId.Combat).NextU32();
            var loot = provider.GetStream(RngStreamId.Loot).NextU32();

            Assert.AreNotEqual(combat, loot);
        }
    }
}
