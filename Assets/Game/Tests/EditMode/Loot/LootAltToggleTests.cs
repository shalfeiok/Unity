using Game.Presentation.Input;
using NUnit.Framework;

namespace Game.Tests.EditMode.Loot
{
    public sealed class LootAltToggleTests
    {
        [Test]
        public void Visibility_FollowsAltAndAlwaysVisibleFlags()
        {
            var toggle = new LootAltToggle();
            Assert.False(toggle.IsLootVisible);

            toggle.SetAltHeld(true);
            Assert.True(toggle.IsLootVisible);

            toggle.SetAltHeld(false);
            Assert.False(toggle.IsLootVisible);

            toggle.SetAlwaysVisible(true);
            Assert.True(toggle.IsLootVisible);
        }
    }
}
