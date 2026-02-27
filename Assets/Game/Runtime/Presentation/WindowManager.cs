using System.Collections.Generic;
using UnityEngine;

namespace Game.Presentation
{
    public sealed class WindowManager : MonoBehaviour
    {
        private readonly List<RectTransform> _windows = new();

        public void Register(RectTransform window)
        {
            if (window == null || _windows.Contains(window))
            {
                return;
            }

            _windows.Add(window);
            Focus(window);
        }

        public void Focus(RectTransform window)
        {
            if (window == null)
            {
                return;
            }

            window.SetAsLastSibling();
        }
    }
}
