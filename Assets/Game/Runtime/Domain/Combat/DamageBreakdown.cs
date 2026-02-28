using Game.Domain.Modifiers;

namespace Game.Domain.Combat
{
    public readonly struct DamageBreakdown
    {
        public DamageBreakdown(ModifierComputation computation)
        {
            Final = computation.FinalValue;
            Added = computation.Add;
            Increased = computation.Increased;
            More = computation.More;
            ConvertedOut = computation.ConvertedOut;
        }

        public float Final { get; }
        public float Added { get; }
        public float Increased { get; }
        public float More { get; }
        public float ConvertedOut { get; }
    }
}
