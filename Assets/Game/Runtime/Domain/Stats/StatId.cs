namespace Game.Domain.Stats
{
    /// <summary>
    /// Stable stat identifiers. In production prefer a registry or generated constants to keep stable IDs across versions.
    /// </summary>
    public enum StatId
    {
        // Primary
        Strength = 1,
        Dexterity = 2,
        Intelligence = 3,

        // Resources
        MaxHp = 100,
        MaxMp = 101,

        // Offense
        Damage = 200,
        CritChance = 201,
        CritMultiplier = 202,
        CastSpeed = 203,

        // Defense
        Armor = 300,
        ResistFire = 310,
        ResistCold = 311,
        ResistPoison = 312,

        // Utility
        MoveSpeed = 400,
        CooldownReduction = 401
    }
}
