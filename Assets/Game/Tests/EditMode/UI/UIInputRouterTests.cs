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

            Assert.False(input.TryHandleToggleKey("I"));
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
        public void TryHandleToggleKey_NotifiesBackNavigation_OnSuccess()
        {
            var registry = new WindowRegistry();
            registry.Register(WindowId.Inventory, _ => { });
            var back = new BackNavigationStub(false, false, false);
            var input = new UIInputRouter(
                new InputContextStack(),
                new UIHotkeyRouter(new WindowManager(new WindowService(registry))),
                backNavigation: back);

            Assert.True(input.TryHandleToggleKey("I"));
            Assert.AreEqual(1, back.NotifyCalls);
            Assert.AreEqual(WindowId.Inventory, back.LastWindowId);
            Assert.True(back.LastIsOpen);
        }

        [Test]
        public void TryHandleToggleKey_DoesNotNotifyBackNavigation_OnResolveFailure()
        {
            var registry = new WindowRegistry();
            registry.Register(WindowId.Inventory, _ => { });
            var back = new BackNavigationStub(false, false, false);
            var resolver = new ConstantResolver(UIHotkey.None, shouldResolve: true);
            var input = new UIInputRouter(
                new InputContextStack(),
                new UIHotkeyRouter(new WindowManager(new WindowService(registry))),
                hotkeyResolver: resolver,
                backNavigation: back);

            Assert.False(input.TryHandleToggleKey("I"));
            Assert.AreEqual(0, back.NotifyCalls);
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
        public void TryHandleEscape_WhenClosingModal_PopsModalContext()
        {
            var registry = new WindowRegistry();
            registry.Register(WindowId.Inventory, _ => { });
            var contexts = new InputContextStack();
            contexts.Push(InputContext.Modal);
            var back = new BackNavigationStub(hasModal: true, closeModalResult: true, closeTopPanelResult: false);
            var input = new UIInputRouter(
                contexts,
                new UIHotkeyRouter(new WindowManager(new WindowService(registry))),
                backNavigation: back);

            Assert.True(input.TryHandleEscape());
            Assert.AreEqual(InputContext.Gameplay, contexts.Current);
            Assert.True(input.TryHandleToggleKey("I"));
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

        [Test]
        public void TryHandleEscape_WithWindowStackBackNavigation_ClosesLastToggledPanel()
        {
            var registry = new WindowRegistry();
            registry.Register(WindowId.Inventory, _ => { });
            var service = new WindowService(registry);
            var manager = new WindowManager(service);
            var contexts = new InputContextStack();
            contexts.Push(InputContext.UI);
            var back = new WindowStackBackNavigation(service);
            var input = new UIInputRouter(contexts, new UIHotkeyRouter(manager), backNavigation: back);

            Assert.True(input.TryHandleToggleKey("I"));
            Assert.True(service.IsOpen(WindowId.Inventory));

            Assert.True(input.TryHandleEscape());
            Assert.False(service.IsOpen(WindowId.Inventory));
        }


        [Test]
        public void OnModalOpened_AndOnModalClosed_TrackModalDepthAndContext()
        {
            var contexts = new InputContextStack();
            var back = new BackNavigationStub(false, false, false);
            var input = new UIInputRouter(
                contexts,
                new UIHotkeyRouter(new WindowManager(new WindowService(new WindowRegistry()))),
                backNavigation: back);

            input.OnModalOpened();
            Assert.AreEqual(InputContext.Modal, contexts.Current);
            Assert.AreEqual(1, back.EnterModalCalls);

            input.OnModalClosed();
            Assert.AreEqual(InputContext.Gameplay, contexts.Current);
            Assert.AreEqual(1, back.ExitModalCalls);
        }

        private sealed class ConstantResolver : IUIHotkeyResolver
        {
            private readonly UIHotkey _hotkey;
            private readonly bool _shouldResolve;

            public ConstantResolver(UIHotkey hotkey, bool shouldResolve)
            {
                _hotkey = hotkey;
                _shouldResolve = shouldResolve;
            }

            public bool TryResolve(string key, out UIHotkey hotkey)
            {
                hotkey = _hotkey;
                return _shouldResolve;
            }
        }

        private sealed class BackNavigationStub : IUIBackNavigation
        {
            private int _modalDepth;
            private readonly bool _closeModalResult;
            private readonly bool _closeTopPanelResult;

            public int CloseModalCalls { get; private set; }
            public int CloseTopPanelCalls { get; private set; }
            public int NotifyCalls { get; private set; }
            public int EnterModalCalls { get; private set; }
            public int ExitModalCalls { get; private set; }
            public WindowId LastWindowId { get; private set; }
            public bool LastIsOpen { get; private set; }

            public BackNavigationStub(bool hasModal, bool closeModalResult, bool closeTopPanelResult)
            {
                _modalDepth = hasModal ? 1 : 0;
                _closeModalResult = closeModalResult;
                _closeTopPanelResult = closeTopPanelResult;
            }

            public bool HasModal() => _modalDepth > 0;

            public bool TryCloseModal()
            {
                CloseModalCalls++;
                if (_closeModalResult && _modalDepth > 0)
                    _modalDepth--;

                return _closeModalResult;
            }

            public bool TryCloseTopPanel()
            {
                CloseTopPanelCalls++;
                return _closeTopPanelResult;
            }

            public bool HasOpenPanels()
            {
                return LastIsOpen;
            }

            public void NotifyWindowState(WindowId windowId, bool isOpen)
            {
                NotifyCalls++;
                LastWindowId = windowId;
                LastIsOpen = isOpen;
            }

            public void EnterModal()
            {
                EnterModalCalls++;
                _modalDepth++;
            }

            public void ExitModal()
            {
                ExitModalCalls++;
                if (_modalDepth > 0)
                    _modalDepth--;
            }
        }
    }
}
