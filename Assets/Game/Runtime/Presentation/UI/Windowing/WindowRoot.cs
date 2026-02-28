using UnityEngine;

namespace Game.Presentation.UI.Windowing
{
    public sealed class WindowRoot : MonoBehaviour
    {
        [SerializeField] private WindowId windowId;

        public WindowId Id => windowId;

        public void SetVisible(bool value) => gameObject.SetActive(value);
    }
}
