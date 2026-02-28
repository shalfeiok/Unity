using System.Collections.Generic;

namespace Game.Domain.Poe.Passives
{
    public sealed class PassiveTreeService
    {
        public bool TryAllocateNode(PassiveTreeDefinition tree, HashSet<string> allocated, string nodeId, int availablePoints)
        {
            if (availablePoints <= 0 || allocated.Contains(nodeId))
                return false;

            var target = FindNode(tree, nodeId);
            if (target == null)
                return false;

            if (target.IsStart)
            {
                allocated.Add(nodeId);
                return true;
            }

            for (int i = 0; i < target.Neighbors.Count; i++)
            {
                if (allocated.Contains(target.Neighbors[i]))
                {
                    allocated.Add(nodeId);
                    return true;
                }
            }

            return false;
        }

        public bool TryRefundNode(PassiveTreeDefinition tree, HashSet<string> allocated, string nodeId)
        {
            if (!allocated.Contains(nodeId)) return false;
            var node = FindNode(tree, nodeId);
            if (node == null || node.IsStart) return false;

            allocated.Remove(nodeId);
            if (HasDisconnectedAllocations(tree, allocated))
            {
                allocated.Add(nodeId);
                return false;
            }

            return true;
        }

        private static PassiveNodeDefinition FindNode(PassiveTreeDefinition tree, string nodeId)
        {
            for (int i = 0; i < tree.Nodes.Count; i++)
                if (tree.Nodes[i].Id == nodeId)
                    return tree.Nodes[i];
            return null;
        }

        private static bool HasDisconnectedAllocations(PassiveTreeDefinition tree, HashSet<string> allocated)
        {
            var visited = new HashSet<string>();
            var stack = new Stack<string>();

            for (int i = 0; i < tree.Nodes.Count; i++)
            {
                if (tree.Nodes[i].IsStart && allocated.Contains(tree.Nodes[i].Id))
                    stack.Push(tree.Nodes[i].Id);
            }

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                if (!visited.Add(current)) continue;

                var node = FindNode(tree, current);
                if (node == null) continue;

                for (int i = 0; i < node.Neighbors.Count; i++)
                {
                    var neighbor = node.Neighbors[i];
                    if (allocated.Contains(neighbor) && !visited.Contains(neighbor))
                        stack.Push(neighbor);
                }
            }

            foreach (var id in allocated)
                if (!visited.Contains(id))
                    return true;

            return false;
        }
    }
}
