using Game.Domain.Modifiers;
using Game.Domain.Poe.Flasks;
using Game.Domain.Stats;
using NUnit.Framework;

namespace Game.Tests.EditMode.Poe
{
    public sealed class FlaskServiceTests
    {
        [Test]
        public void TryUse_ConsumesCharges_WhenEnough()
        {
            var flask = new FlaskDefinition { Id = "life_flask", MaxCharges = 30, ChargesPerUse = 10, Duration = 4f };
            var service = new FlaskService();
            service.Initialize(flask);

            Assert.True(service.TryUse(flask));
            Assert.AreEqual(20, service.GetCharges(flask.Id));
        }

        [Test]
        public void GainCharges_ClampsToMax()
        {
            var flask = new FlaskDefinition { Id = "mana_flask", MaxCharges = 20, ChargesPerUse = 5, Duration = 4f };
            var service = new FlaskService();
            service.Initialize(flask);
            service.TryUse(flask);
            service.GainCharges(flask.Id, 100, flask.MaxCharges);

            Assert.AreEqual(20, service.GetCharges(flask.Id));
        }

        [Test]
        public void BuildEffectModifiers_ReturnsModifierV2Set()
        {
            var flask = new FlaskDefinition
            {
                Id = "granite",
                Effects =
                {
                    new FlaskEffectDefinition { Stat = StatId.Armor, Bucket = ModifierBucket.Add, Value = 100f },
                    new FlaskEffectDefinition { Stat = StatId.MoveSpeed, Bucket = ModifierBucket.Increased, Value = 0.1f }
                }
            };

            var service = new FlaskService();
            var mods = service.BuildEffectModifiers(flask, sourceId: 777);

            Assert.AreEqual(2, mods.Count);
            Assert.AreEqual(StatId.Armor, mods[0].Stat);
            Assert.AreEqual(777, mods[0].SourceId);
            Assert.AreEqual(ModifierBucket.Increased, mods[1].Bucket);
        }
    }
}
