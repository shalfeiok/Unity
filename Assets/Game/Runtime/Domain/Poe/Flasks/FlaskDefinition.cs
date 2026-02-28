using System.Collections.Generic;
using Game.Domain.Modifiers;
using Game.Domain.Stats;
using Game.Domain.Tags;

namespace Game.Domain.Poe.Flasks
{
    public sealed class FlaskDefinition
    {
        public string Id;
        public int MaxCharges;
        public int ChargesPerUse;
        public float Duration;
        public List<FlaskEffectDefinition> Effects = new();
    }

    public sealed class FlaskEffectDefinition
    {
        public StatId Stat;
        public ModifierBucket Bucket;
        public float Value;
        public TagId ScopeTag;
    }
}
