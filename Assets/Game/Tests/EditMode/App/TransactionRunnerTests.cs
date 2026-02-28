using Game.Application.Transactions;
using NUnit.Framework;

namespace Game.Tests.EditMode.App
{
    public sealed class TransactionRunnerTests
    {
        [Test]
        public void Run_IsIdempotentByOperationId()
        {
            var runner = new TransactionRunner();
            int applied = 0;

            Assert.True(runner.Run("op_1", () => true, () => applied++, null));
            Assert.True(runner.Run("op_1", () => true, () => applied++, null));

            Assert.AreEqual(1, applied);
        }

        [Test]
        public void Run_StopsWhenValidationFails()
        {
            var runner = new TransactionRunner();
            int applied = 0;

            Assert.False(runner.Run("op_2", () => false, () => applied++, null));
            Assert.AreEqual(0, applied);
        }
    }
}
