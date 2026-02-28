namespace Game.Domain.Poe.Crafting
{
    public enum CurrencyActionKind
    {
        RerollAllMods = 0,
        AddRandomPrefix = 1,
        AddRandomSuffix = 2,
        RemoveRandomMod = 3
    }

    public sealed class CurrencyActionDefinition
    {
        public string Id;
        public CurrencyActionKind Kind;
        public int Cost;
    }
}
