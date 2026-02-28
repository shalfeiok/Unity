using Game.Domain.Stats;
using Game.Domain.Tags;

namespace Game.Domain.Modifiers
{
    public readonly struct Modifier
    {
        public Modifier(
            StatId stat,
            ModifierBucket bucket,
            float value,
            int sourceId,
            TagId scopeTag,
            StatId conversionTarget = 0)
        {
            Stat = stat;
            Bucket = bucket;
            Value = value;
            SourceId = sourceId;
            ScopeTag = scopeTag;
            ConversionTarget = conversionTarget;
        }

        public StatId Stat { get; }
        public ModifierBucket Bucket { get; }
        public float Value { get; }
        public int SourceId { get; }
        public TagId ScopeTag { get; }
        public StatId ConversionTarget { get; }
    }
}
