using UnityEngine;

namespace Game.Presentation.UI.Widgets
{
    public sealed class LootLabel : MonoBehaviour
    {
        [SerializeField] private string text;
        [SerializeField] private bool highlighted;

        public string Text => text;
        public bool Highlighted => highlighted;

        public void Bind(string value)
        {
            text = value;
        }

        public void SetHighlighted(bool value)
        {
            highlighted = value;
            gameObject.SetActive(value || !string.IsNullOrEmpty(text));
        }
    }
}
