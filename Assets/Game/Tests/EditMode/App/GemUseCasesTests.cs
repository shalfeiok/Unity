using Game.Application.Poe.UseCases;
using Game.Application.Transactions;
using Game.Domain.Poe.Sockets;
using NUnit.Framework;

namespace Game.Tests.EditMode.App
{
    public sealed class GemUseCasesTests
    {
        [Test]
        public void InsertAndRemoveGem_UseCases_WorkWithTransactionRunner()
        {
            var sockets = new SocketModel(3);
            var runner = new TransactionRunner();
            var socketService = new SocketService();
            var insert = new InsertGemUseCase(runner, socketService);
            var remove = new RemoveGemUseCase(runner, socketService);

            Assert.True(insert.Execute("op_insert_1", sockets, 0, "skill_fireball"));
            Assert.False(insert.Execute("op_insert_1", sockets, 1, "support_chain")); // idempotent op id
            Assert.True(remove.Execute("op_remove_1", sockets, 0, out var removed));
            Assert.AreEqual("skill_fireball", removed);
        }
    }
}
