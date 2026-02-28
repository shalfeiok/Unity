using System.Collections.Generic;

namespace Game.Domain.Poe.Passives
{
    public sealed class PassiveTreeDefinition
    {
        public List<PassiveNodeDefinition> Nodes = new();
    }

    public sealed class PassiveNodeDefinition
    {
        public string Id;
        public bool IsStart;
        public List<string> Neighbors = new();
        public int PointCost = 1;
    }
}
