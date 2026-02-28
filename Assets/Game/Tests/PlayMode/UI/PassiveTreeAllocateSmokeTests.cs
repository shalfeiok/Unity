using System.Collections.Generic;
using Game.Application.Poe.UseCases;
using Game.Application.Transactions;
using Game.Domain.Poe.Passives;
using NUnit.Framework;

namespace Game.Tests.PlayMode.UI
{
    public sealed class PassiveTreeAllocateSmokeTests
    {
        [Test]
        public void AllocatePassiveNodes_ThroughUseCase_Smoke()
        {
            var tree = new PassiveTreeDefinition();
            tree.Nodes.Add(new PassiveNodeDefinition { Id = "start", IsStart = true, Neighbors = new List<string> { "n1" } });
            tree.Nodes.Add(new PassiveNodeDefinition { Id = "n1", Neighbors = new List<string> { "start" } });

            var allocated = new HashSet<string>();
            var useCase = new AllocatePassiveNodeUseCase(new TransactionRunner(), new PassiveTreeService());

            Assert.True(useCase.Execute("pm_passive_start", tree, allocated, "start", 1));
            Assert.True(useCase.Execute("pm_passive_n1", tree, allocated, "n1", 1));
        }
    }
}
