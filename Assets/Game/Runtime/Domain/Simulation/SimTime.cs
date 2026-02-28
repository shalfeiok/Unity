namespace Game.Domain.Simulation
{
    public readonly struct SimTime
    {
        public SimTime(long tickIndex, float deltaTime)
        {
            TickIndex = tickIndex;
            DeltaTime = deltaTime;
        }

        public long TickIndex { get; }
        public float DeltaTime { get; }

        public float ElapsedTime => TickIndex * DeltaTime;

        public SimTime NextTick() => new(TickIndex + 1, DeltaTime);
    }
}
