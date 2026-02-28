using Game.Presentation.UI.Windowing;
using NUnit.Framework;

namespace Game.Tests.EditMode.UI
{
    public sealed class UIInputRouterTests
    {
        [Test]
        public void TryHandleToggleKey_WhenGameplay_TogglesWindow()
        {
            var registry = new WindowRegistry();
            var service = new WindowService(registry);
            var manager = new WindowManager(service);
            var input = new UIInputRouter(new InputContextStack(), new UIHotkeyRouter(manager));

            Assert.True(input.TryHandleToggleKey("I"));
            Assert.True(service.IsOpen(WindowId.Inventory));

            Assert.True(input.TryHandleToggleKey("I"));
            Assert.False(service.IsOpen(WindowId.Inventory));
        }

        [Test]
        public void TryHandleToggleKey_WhenModal_DoesNotToggle()
        {
            var registry = new WindowRegistry();
            var service = new WindowService(registry);
            var manager = new WindowManager(service);
            var contexts = new InputContextStack();
            contexts.Push(InputContext.Modal);
            var input = new UIInputRouter(contexts, new UIHotkeyRouter(manager));

            Assert.False(input.TryHandleToggleKey("I"));
            Assert.False(service.IsOpen(WindowId.Inventory));
        }

        [Test]
        public void TryHandleToggleKey_UnknownKey_ReturnsFalse()
        {
            var registry = new WindowRegistry();
            var service = new WindowService(registry);
            var manager = new WindowManager(service);
            var input = new UIInputRouter(new InputContextStack(), new UIHotkeyRouter(manager));

            Assert.False(input.TryHandleToggleKey("F13"));
            Assert.False(service.IsOpen(WindowId.Inventory));
        }
    }
}
