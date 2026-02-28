using UnityEngine;

namespace Game.Presentation.World
{
    public sealed class LootWorldActor : MonoBehaviour
    {
        [SerializeField] private string itemId;
        [SerializeField] private int quantity = 1;

        public string ItemId => itemId;
        public int Quantity => quantity;

        public void Initialize(string value, int amount)
        {
            itemId = value;
            quantity = amount;
        }
    }
}
