namespace Game.Domain.Poe.Crafting
{
    public enum CurrencyActionKind
    {
        RerollAllMods = 0,
        AddRandomPrefix = 1,
        AddRandomSuffix = 2,
        RemoveRandomMod = 3,
        ReforgeWithBias = 4,
        SetSocketCount = 5,
        RerollSocketColors = 6,
        RerollLinks = 7,
        ImproveQuality = 8,
        Corrupt = 9
    }

    public sealed class CurrencyActionDefinition
    {
        public string Id;
        public CurrencyActionKind Kind;
        public int Cost;
        public string BiasTag;
        public int IntValue;
    }
}
