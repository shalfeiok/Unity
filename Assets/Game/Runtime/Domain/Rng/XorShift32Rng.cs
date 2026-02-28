namespace Game.Domain.Rng
{
    /// <summary>Deterministic RNG for gameplay. Do NOT use UnityEngine.Random in Domain.</summary>
    public sealed class XorShift32Rng : IRng
    {
        private uint _state;

        public XorShift32Rng(uint seed)
        {
            _state = seed == 0 ? 0xCAFEBABEu : seed;
        }

        public uint NextU32()
        {
            uint x = _state;
            x ^= x << 13;
            x ^= x >> 17;
            x ^= x << 5;
            _state = x;
            return x;
        }

        public float Next01()
        {
            // 24-bit mantissa-like
            return (NextU32() & 0xFFFFFFu) / 16777216f;
        }

        public int Range(int minInclusive, int maxExclusive)
        {
            if (maxExclusive <= minInclusive) return minInclusive;
            uint span = (uint)(maxExclusive - minInclusive);
            return (int)(NextU32() % span) + minInclusive;
        }
    }
}
