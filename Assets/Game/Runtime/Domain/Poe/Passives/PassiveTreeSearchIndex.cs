using System;
using System.Collections.Generic;

namespace Game.Domain.Poe.Passives
{
    public sealed class PassiveTreeSearchIndex
    {
        private readonly Dictionary<string, HashSet<string>> _tokenToNodeIds = new(StringComparer.OrdinalIgnoreCase);

        public PassiveTreeSearchIndex(PassiveTreeDefinition tree)
        {
            if (tree == null)
                return;

            for (int i = 0; i < tree.Nodes.Count; i++)
            {
                var node = tree.Nodes[i];
                if (string.IsNullOrWhiteSpace(node?.Id))
                    continue;

                AddToken(node.Id, node.Id);
                AddToken(node.Name, node.Id);

                if (node.Tags == null)
                    continue;

                for (int t = 0; t < node.Tags.Count; t++)
                    AddToken(node.Tags[t], node.Id);
            }
        }

        public IReadOnlyList<string> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return Array.Empty<string>();

            var matches = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var kv in _tokenToNodeIds)
            {
                if (kv.Key.IndexOf(query, StringComparison.OrdinalIgnoreCase) < 0)
                    continue;

                foreach (var id in kv.Value)
                    matches.Add(id);
            }

            if (matches.Count == 0)
                return Array.Empty<string>();

            var result = new List<string>(matches);
            result.Sort(StringComparer.OrdinalIgnoreCase);
            return result;
        }

        private void AddToken(string token, string nodeId)
        {
            if (string.IsNullOrWhiteSpace(token))
                return;

            if (!_tokenToNodeIds.TryGetValue(token, out var nodeIds))
            {
                nodeIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                _tokenToNodeIds[token] = nodeIds;
            }

            nodeIds.Add(nodeId);
        }
    }
}
