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
        public string Name;
        public List<string> Tags = new();
        public bool IsStart;
        public List<string> Neighbors = new();
        public int PointCost = 1;
    }
}
