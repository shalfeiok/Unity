using Game.Infrastructure.Economy;
using NUnit.Framework;

namespace Game.Tests.EditMode.App
{
    public sealed class TransactionLedgerTests
    {
        [Test]
        public void Append_AddsEntriesInOrder()
        {
            var ledger = new TransactionLedger();
            ledger.Append("op1", "action1", "item1", 1);
            ledger.Append("op2", "action2", "item2", 2);

            Assert.AreEqual(2, ledger.Entries.Count);
            Assert.AreEqual("op1", ledger.Entries[0].OperationId);
            Assert.AreEqual("op2", ledger.Entries[1].OperationId);
        }
    }
}
