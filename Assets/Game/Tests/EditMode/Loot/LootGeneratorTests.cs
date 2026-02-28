using Game.Domain.Loot;
using Game.Domain.Rng;
using NUnit.Framework;

namespace Game.Tests.EditMode.Loot
{
    public sealed class LootGeneratorTests
    {
        [Test]
        public void GenerateDrops_IsDeterministicForSameSeed()
        {
            var a = new LootGenerator(new XorShift32Rng(123));
            var b = new LootGenerator(new XorShift32Rng(123));

            var da = a.GenerateDrops(25, 20);
            var db = b.GenerateDrops(25, 20);

            Assert.AreEqual(da.Count, db.Count);
            for (int i = 0; i < da.Count; i++)
            {
                Assert.AreEqual(da[i].ItemId, db[i].ItemId);
                Assert.AreEqual(da[i].Rarity, db[i].Rarity);
                Assert.AreEqual(da[i].Quantity, db[i].Quantity);
            }
        }

        [Test]
        public void GenerateDrops_ReturnsRequestedCount()
        {
            var generator = new LootGenerator(new XorShift32Rng(7));
            var drops = generator.GenerateDrops(10, 5);

            Assert.AreEqual(5, drops.Count);
        }
    }
}
