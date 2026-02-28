namespace Game.Application.Events
{
    public enum ApplicationEventType
    {
        Unknown = 0,
        GemInserted = 1,
        GemRemoved = 2,
        PassiveAllocated = 3,
        PassiveRefunded = 4,
        CurrencyApplied = 5,
        FlaskUsed = 6,
        HotbarAssigned = 7,
        HotbarUnassigned = 8,
        LootPickedUp = 9
    }
}
