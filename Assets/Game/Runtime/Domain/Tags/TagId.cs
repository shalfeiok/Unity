namespace Game.Domain.Tags
{
    /// <summary>
    /// Stable identifier for a gameplay tag. Prefer authoring tags in a registry and referencing by int.
    /// </summary>
    public readonly struct TagId
    {
        public readonly int Value;

        public TagId(int value) => Value = value;

        public bool Equals(TagId other) => Value == other.Value;
        public override bool Equals(object obj) => obj is TagId other && Equals(other);
        public override int GetHashCode() => Value;
        public override string ToString() => Value.ToString();
    }
}
