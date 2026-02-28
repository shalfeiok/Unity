using System.Collections.Generic;
using Game.Application.Poe.UseCases;
using Game.Application.Transactions;
using Game.Domain.Poe.Passives;
using NUnit.Framework;

namespace Game.Tests.EditMode.App
{
    public sealed class PassiveRefundUseCaseTests
    {
        [Test]
        public void RefundPassiveNode_UseCase_RespectsConnectivityRules()
        {
            var tree = new PassiveTreeDefinition();
            tree.Nodes.Add(new PassiveNodeDefinition { Id = "start", IsStart = true, Neighbors = new List<string> { "n1" } });
            tree.Nodes.Add(new PassiveNodeDefinition { Id = "n1", Neighbors = new List<string> { "start", "n2" } });
            tree.Nodes.Add(new PassiveNodeDefinition { Id = "n2", Neighbors = new List<string> { "n1" } });

            var allocated = new HashSet<string> { "start", "n1", "n2" };
            var useCase = new RefundPassiveNodeUseCase(new TransactionRunner(), new PassiveTreeService());

            Assert.False(useCase.Execute("op_refund_n1", tree, allocated, "n1"));
            Assert.True(useCase.Execute("op_refund_n2", tree, allocated, "n2"));
            Assert.False(allocated.Contains("n2"));
        }
    }
}
