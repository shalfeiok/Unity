using System.Collections.Generic;
using Game.Domain.Poe.Passives;

namespace Game.Infrastructure.DataPipeline
{
    public sealed class PassiveTreeValidator
    {
        public bool Validate(PassiveTreeDefinition tree, out string error)
        {
            var ids = new HashSet<string>();
            int starts = 0;

            for (int i = 0; i < tree.Nodes.Count; i++)
            {
                var node = tree.Nodes[i];
                if (string.IsNullOrWhiteSpace(node.Id))
                {
                    error = "Passive node id is empty.";
                    return false;
                }

                if (!ids.Add(node.Id))
                {
                    error = $"Duplicate passive node id: {node.Id}";
                    return false;
                }

                if (node.IsStart) starts++;
            }

            if (starts == 0)
            {
                error = "Passive tree has no start node.";
                return false;
            }

            error = string.Empty;
            return true;
        }
    }
}
