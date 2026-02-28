using System;
using System.Collections.Generic;

namespace Game.Domain.Rng
{
    public sealed class RngProvider
    {
        private readonly uint _masterSeed;
        private readonly Dictionary<RngStreamId, IRng> _streams = new();

        public RngProvider(uint masterSeed)
        {
            _masterSeed = masterSeed == 0 ? 0xA341316Cu : masterSeed;
        }

        public IRng GetStream(RngStreamId streamId)
        {
            if (_streams.TryGetValue(streamId, out var stream))
                return stream;

            uint subSeed = DeriveSeed(_masterSeed, streamId);
            stream = new XorShift32Rng(subSeed);
            _streams.Add(streamId, stream);
            return stream;
        }

        private static uint DeriveSeed(uint masterSeed, RngStreamId streamId)
        {
            ulong mixed = ((ulong)masterSeed << 32) | (uint)streamId;
            mixed += 0x9E3779B97F4A7C15UL;
            mixed = (mixed ^ (mixed >> 30)) * 0xBF58476D1CE4E5B9UL;
            mixed = (mixed ^ (mixed >> 27)) * 0x94D049BB133111EBUL;
            mixed ^= mixed >> 31;

            uint seed = (uint)(mixed & uint.MaxValue);
            return seed == 0 ? 0x6D2B79F5u : seed;
        }
    }
}
