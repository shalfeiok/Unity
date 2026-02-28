using Game.Domain.Poe.Flasks;
using Game.Presentation.UI.Hud.Flasks;
using NUnit.Framework;

namespace Game.Tests.EditMode.UI.Hud
{
    public sealed class FlaskBeltHudServiceTests
    {
        [Test]
        public void RefreshSlot_ReflectsChargesAndUsability()
        {
            var flask = new FlaskDefinition { Id = "life", MaxCharges = 30, ChargesPerUse = 10, Duration = 4f };
            var flaskService = new FlaskService();
            flaskService.Initialize(flask);
            flaskService.TryUse(flask); // now 20

            var hud = new FlaskBeltHudService(flaskService);
            var slot = new FlaskSlotState();
            hud.RefreshSlot(slot, flask);

            Assert.AreEqual("life", slot.FlaskId);
            Assert.AreEqual(30, slot.MaxCharges);
            Assert.AreEqual(20, slot.Charges);
            Assert.True(slot.IsUsable);
        }
    }
}
