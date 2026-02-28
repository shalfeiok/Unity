using System.Collections.Generic;

namespace Game.Presentation.UI.Windows.Inventory
{
    public sealed class InventoryWindowState
    {
        public List<string> ItemIds { get; } = new();
        public List<string> GemIds { get; } = new();

        public bool RemoveGem(string gemId) => GemIds.Remove(gemId);
        public void AddGem(string gemId)
        {
            if (!string.IsNullOrWhiteSpace(gemId))
                GemIds.Add(gemId);
        }
    }
}
