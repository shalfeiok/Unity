namespace Game.Domain.Poe.Items
{
    public sealed class ModDefinition
    {
        public string Id;
        public string Group;
        public bool IsPrefix;
        public int Tier;
        public int MinItemLevel;
        public float MinValue;
        public float MaxValue;
    }
}
