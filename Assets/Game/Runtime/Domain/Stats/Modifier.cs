namespace Game.Domain.Stats
{
    public readonly struct Modifier
    {
        public readonly StatId Stat;
        public readonly ModifierOp Op;
        public readonly float Value;
        public readonly int Bucket;
        public readonly int SourceId; // ItemId/BuffId/AspectId etc (stable int)

        public Modifier(StatId stat, ModifierOp op, float value, int bucket, int sourceId)
        {
            Stat = stat;
            Op = op;
            Value = value;
            Bucket = bucket;
            SourceId = sourceId;
        }
    }
}
