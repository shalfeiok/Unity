using System.Collections.Generic;

namespace Game.Domain.Poe.Items
{
    public sealed class ItemBaseDefinition
    {
        public string Id;
        public string ItemClass;
        public int RequiredItemLevel;
        public int MaxSockets = 6;
        public List<ModDefinition> ImplicitMods = new();
    }
}
