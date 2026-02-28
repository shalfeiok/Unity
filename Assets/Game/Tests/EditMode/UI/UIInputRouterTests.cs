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

        [Test]
        public void TryHandleEscape_WhenModalOpen_ClosesModalFirst()
        {
            var back = new BackNavigationStub(hasModal: true, closeModalResult: true, closeTopPanelResult: true);
            var input = new UIInputRouter(
                new InputContextStack(),
                new UIHotkeyRouter(new WindowManager(new WindowService(new WindowRegistry()))),
                backNavigation: back);

            Assert.True(input.TryHandleEscape());
            Assert.AreEqual(1, back.CloseModalCalls);
            Assert.AreEqual(0, back.CloseTopPanelCalls);
        }

        [Test]
        public void TryHandleEscape_WhenUiContext_ClosesTopPanel()
        {
            var contexts = new InputContextStack();
            contexts.Push(InputContext.UI);
            var back = new BackNavigationStub(hasModal: false, closeModalResult: false, closeTopPanelResult: true);
            var input = new UIInputRouter(
                contexts,
                new UIHotkeyRouter(new WindowManager(new WindowService(new WindowRegistry()))),
                backNavigation: back);

            Assert.True(input.TryHandleEscape());
            Assert.AreEqual(0, back.CloseModalCalls);
            Assert.AreEqual(1, back.CloseTopPanelCalls);
        }

        private sealed class BackNavigationStub : IUIBackNavigation
        {
            private readonly bool _hasModal;
            private readonly bool _closeModalResult;
            private readonly bool _closeTopPanelResult;

            public int CloseModalCalls { get; private set; }
            public int CloseTopPanelCalls { get; private set; }

            public BackNavigationStub(bool hasModal, bool closeModalResult, bool closeTopPanelResult)
            {
                _hasModal = hasModal;
                _closeModalResult = closeModalResult;
                _closeTopPanelResult = closeTopPanelResult;
            }

            public bool HasModal() => _hasModal;

            public bool TryCloseModal()
            {
                CloseModalCalls++;
                return _closeModalResult;
            }

            public bool TryCloseTopPanel()
            {
                CloseTopPanelCalls++;
                return _closeTopPanelResult;
            }
        }
    }
}
