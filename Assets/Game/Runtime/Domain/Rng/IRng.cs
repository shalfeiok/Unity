namespace Game.Domain.Rng
{
    public interface IRng
    {
        /// <summary>Returns unsigned 32-bit value.</summary>
        uint NextU32();

        /// <summary>Returns float in [0,1).</summary>
        float Next01();

        /// <summary>Returns int in [minInclusive, maxExclusive).</summary>
        int Range(int minInclusive, int maxExclusive);
    }
}
