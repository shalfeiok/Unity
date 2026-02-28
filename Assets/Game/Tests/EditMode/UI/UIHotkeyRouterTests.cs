using Game.Presentation.UI.Windowing;
using NUnit.Framework;

namespace Game.Tests.EditMode.UI
{
    public sealed class UIHotkeyRouterTests
    {
        [Test]
        public void TryResolveWindow_MapsAtlasAndCraftHotkeys()
        {
            var router = new UIHotkeyRouter(new WindowManager(new WindowService(new WindowRegistry())));

            Assert.True(router.TryResolveWindow(UIHotkey.Atlas, out var atlasWindow));
            Assert.AreEqual(WindowId.Atlas, atlasWindow);

            Assert.True(router.TryResolveWindow(UIHotkey.SkillcraftForge, out var forgeWindow));
            Assert.AreEqual(WindowId.Craft, forgeWindow);

            Assert.True(router.TryResolveWindow(UIHotkey.CraftingBench, out var benchWindow));
            Assert.AreEqual(WindowId.Craft, benchWindow);
        }

        [Test]
        public void TryToggle_UnknownHotkey_ReturnsFalse()
        {
            var router = new UIHotkeyRouter(new WindowManager(new WindowService(new WindowRegistry())));
            Assert.False(router.TryToggle(UIHotkey.None));
        }
    }
}
