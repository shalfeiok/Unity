using System.Collections.Generic;
using Game.Presentation.UI.Windowing;
using NUnit.Framework;

namespace Game.Tests.EditMode.UI
{
    public sealed class UIHotkeyRouterTests
    {

        [Test]
        public void Ctor_NullWindowManager_Throws()
        {
            Assert.Throws<System.ArgumentNullException>(() => new UIHotkeyRouter(null));
        }
        [Test]
        public void TryResolveWindow_UsesDefaultBindings_ForAtlasAndCraftHotkeys()
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

        [Test]
        public void TryResolveWindow_UsesInjectedBindings()
        {
            var customBindings = new DefaultUIHotkeyBindings(
                new Dictionary<UIHotkey, WindowId>
                {
                    [UIHotkey.Inventory] = WindowId.Atlas
                });
            var router = new UIHotkeyRouter(new WindowManager(new WindowService(new WindowRegistry())), customBindings);

            Assert.True(router.TryResolveWindow(UIHotkey.Inventory, out var resolved));
            Assert.AreEqual(WindowId.Atlas, resolved);
        }
    }
}
