namespace Game.Domain.Poe.Gems
{
    public sealed class SupportGemDefinition
    {
        public string Id;
        public SupportEffectKind Effect;
        public string[] RequiredTags;
        public float Value;
    }
}
