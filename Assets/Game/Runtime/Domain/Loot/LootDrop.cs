namespace Game.Domain.Loot
{
    public readonly struct LootDrop
    {
        public LootDrop(string itemId, LootRarity rarity, int quantity)
        {
            ItemId = itemId;
            Rarity = rarity;
            Quantity = quantity;
        }

        public string ItemId { get; }
        public LootRarity Rarity { get; }
        public int Quantity { get; }
    }
}
