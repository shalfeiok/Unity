using Game.Domain.Modifiers;

namespace Game.Domain.Combat
{
    public readonly struct DamageBreakdown
    {
        public DamageBreakdown(ModifierComputation computation, float baseDamage, float critChance = 0f, float critMultiplier = 1.5f, float mitigation = 0f)
        {
            Base = baseDamage;
            Added = computation.Add;
            Increased = computation.Increased;
            More = computation.More;
            BeforeConversion = computation.ValueBeforeConversion;
            ConvertedOut = computation.ConvertedOut;
            GainedAsExtra = computation.GainedAsExtra;
            AfterConversionAndExtra = computation.FinalValue;

            CritChance = Clamp01(critChance);
            CritMultiplier = critMultiplier < 1f ? 1f : critMultiplier;
            CritExpectedMultiplier = 1f + CritChance * (CritMultiplier - 1f);
            AfterCrit = AfterConversionAndExtra * CritExpectedMultiplier;

            Mitigation = Clamp01(mitigation);
            MitigationMultiplier = 1f - Mitigation;
            Final = AfterCrit * MitigationMultiplier;
        }

        public float Base { get; }
        public float Added { get; }
        public float Increased { get; }
        public float More { get; }
        public float BeforeConversion { get; }
        public float ConvertedOut { get; }
        public float GainedAsExtra { get; }
        public float AfterConversionAndExtra { get; }
        public float CritChance { get; }
        public float CritMultiplier { get; }
        public float CritExpectedMultiplier { get; }
        public float AfterCrit { get; }
        public float Mitigation { get; }
        public float MitigationMultiplier { get; }
        public float Final { get; }

        private static float Clamp01(float v)
        {
            if (v < 0f) return 0f;
            if (v > 1f) return 1f;
            return v;
        }
    }
}
