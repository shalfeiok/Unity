using Game.Presentation.UI.Windowing;
using NUnit.Framework;

namespace Game.Tests.EditMode.UI
{
    public sealed class WindowStackBackNavigationTests
    {
        [Test]
        public void TryCloseTopPanel_ClosesLastOpenedWindow()
        {
            var registry = new WindowRegistry();
            bool inventoryVisible = false;
            bool skillsVisible = false;
            registry.Register(WindowId.Inventory, v => inventoryVisible = v);
            registry.Register(WindowId.Skills, v => skillsVisible = v);

            var service = new WindowService(registry);
            service.Open(WindowId.Inventory);
            service.Open(WindowId.Skills);

            var back = new WindowStackBackNavigation(service);
            back.NotifyWindowState(WindowId.Inventory, isOpen: true);
            back.NotifyWindowState(WindowId.Skills, isOpen: true);

            Assert.True(back.TryCloseTopPanel());
            Assert.True(inventoryVisible);
            Assert.False(skillsVisible);
            Assert.True(service.IsOpen(WindowId.Inventory));
            Assert.False(service.IsOpen(WindowId.Skills));
        }

        [Test]
        public void ModalDepth_PushAndClose_Works()
        {
            var back = new WindowStackBackNavigation(new WindowService(new WindowRegistry()));
            back.EnterModal();
            back.EnterModal();

            Assert.True(back.HasModal());
            Assert.True(back.TryCloseModal());
            Assert.True(back.HasModal());
            Assert.True(back.TryCloseModal());
            Assert.False(back.HasModal());
        }
    }
}
