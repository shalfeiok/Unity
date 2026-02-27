using System;
using System.Collections.Generic;

namespace Game.Domain
{
    public enum RngStreamId
    {
        Combat,
        Loot,
        AI,
        World,
        Craft
    }

    public sealed class FixedTickClock
    {
        public int TickRate { get; }
        public long CurrentTick { get; private set; }

        public FixedTickClock(int tickRate)
        {
            if (tickRate <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(tickRate));
            }

            TickRate = tickRate;
        }

        public void Advance(int ticks = 1)
        {
            if (ticks < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(ticks));
            }

            CurrentTick += ticks;
        }
    }

    public sealed class DeterministicRngStreams
    {
        private readonly Dictionary<RngStreamId, Random> _streams = new();

        public DeterministicRngStreams(int seed)
        {
            foreach (RngStreamId stream in Enum.GetValues(typeof(RngStreamId)))
            {
                _streams[stream] = new Random(HashCode.Combine(seed, (int)stream));
            }
        }

        public int NextInt(RngStreamId stream, int minInclusive, int maxExclusive) =>
            _streams[stream].Next(minInclusive, maxExclusive);
    }

    public readonly record struct LedgerEntry(Guid TransactionId, string UseCase, long Tick, DateTime UtcTime, string Payload);
}
