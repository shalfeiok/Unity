using Game.Presentation.UI.Windowing;
using NUnit.Framework;

namespace Game.Tests.EditMode
{
    public sealed class WindowServiceTests
    {
        [Test]
        public void OpenClose_TogglesVisibilityCallbacksAndState()
        {
            var registry = new WindowRegistry();
            bool visible = false;
            registry.Register(WindowId.Inventory, value => visible = value);

            var service = new WindowService(registry);
            service.Open(WindowId.Inventory);
            Assert.True(service.IsOpen(WindowId.Inventory));
            Assert.True(visible);

            service.Close(WindowId.Inventory);
            Assert.False(service.IsOpen(WindowId.Inventory));
            Assert.False(visible);
        }
    }
}
