using System.Collections.Generic;
using Game.Domain.Poe.Passives;
using Game.Infrastructure.DataPipeline;
using NUnit.Framework;

namespace Game.Tests.EditMode.Poe
{
    public sealed class PassiveTreeRulesTests
    {
        [Test]
        public void Allocate_RequiresConnectionToAllocatedNode()
        {
            var tree = BuildTree();
            var allocated = new HashSet<string>();
            var service = new PassiveTreeService();

            Assert.True(service.TryAllocateNode(tree, allocated, "start", 1));
            Assert.True(service.TryAllocateNode(tree, allocated, "n1", 1));
            Assert.False(service.TryAllocateNode(tree, allocated, "isolated", 1));
        }

        [Test]
        public void Refund_BlocksIfItDisconnectsTree()
        {
            var tree = BuildTree();
            var allocated = new HashSet<string> { "start", "n1", "n2" };
            var service = new PassiveTreeService();

            Assert.False(service.TryRefundNode(tree, allocated, "n1"));
            Assert.True(service.TryRefundNode(tree, allocated, "n2"));
        }

        [Test]
        public void Validator_RequiresStartNode()
        {
            var tree = new PassiveTreeDefinition();
            tree.Nodes.Add(new PassiveNodeDefinition { Id = "a", IsStart = false });

            bool ok = new PassiveTreeValidator().Validate(tree, out var error);
            Assert.False(ok);
            Assert.IsNotEmpty(error);
        }

        private static PassiveTreeDefinition BuildTree()
        {
            var tree = new PassiveTreeDefinition();
            tree.Nodes.Add(new PassiveNodeDefinition { Id = "start", IsStart = true, Neighbors = new List<string> { "n1" } });
            tree.Nodes.Add(new PassiveNodeDefinition { Id = "n1", Neighbors = new List<string> { "start", "n2" } });
            tree.Nodes.Add(new PassiveNodeDefinition { Id = "n2", Neighbors = new List<string> { "n1" } });
            tree.Nodes.Add(new PassiveNodeDefinition { Id = "isolated", Neighbors = new List<string>() });
            return tree;
        }
    }
}
