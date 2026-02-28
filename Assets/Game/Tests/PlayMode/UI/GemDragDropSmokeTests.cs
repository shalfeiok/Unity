using Game.Application.Poe.UseCases;
using Game.Application.Transactions;
using Game.Domain.Poe.Sockets;
using NUnit.Framework;

namespace Game.Tests.PlayMode.UI
{
    public sealed class GemDragDropSmokeTests
    {
        [Test]
        public void DragDropGem_InsertAndRemove_ThroughUseCases()
        {
            var sockets = new SocketModel(2);
            var runner = new TransactionRunner();
            var socketService = new SocketService();
            var insert = new InsertGemUseCase(runner, socketService);
            var remove = new RemoveGemUseCase(runner, socketService);

            Assert.True(insert.Execute("pm_drag_insert_1", sockets, 0, "gem_fireball"));
            Assert.True(remove.Execute("pm_drag_remove_1", sockets, 0, out var removed));
            Assert.AreEqual("gem_fireball", removed);
        }
    }
}
