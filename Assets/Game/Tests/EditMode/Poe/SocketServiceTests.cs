using Game.Domain.Poe.Sockets;
using NUnit.Framework;

namespace Game.Tests.EditMode.Poe
{
    public sealed class SocketServiceTests
    {
        [Test]
        public void InsertGem_Fails_WhenSocketOccupied()
        {
            var sockets = new SocketModel(2);
            var service = new SocketService();

            Assert.True(service.TryInsertGem(sockets, 0, "gem_a"));
            Assert.False(service.TryInsertGem(sockets, 0, "gem_b"));
        }

        [Test]
        public void RemoveGem_ReturnsRemovedGemId()
        {
            var sockets = new SocketModel(2);
            var service = new SocketService();
            service.TryInsertGem(sockets, 1, "gem_z");

            Assert.True(service.TryRemoveGem(sockets, 1, out var removed));
            Assert.AreEqual("gem_z", removed);
            Assert.AreEqual(string.Empty, sockets.GemIds[1]);
        }
    }
}
