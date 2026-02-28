using UnityEngine;

namespace Game.Presentation.UI.Windowing
{
    public static class WindowDragMove
    {
        public static Vector2 ApplyDrag(Vector2 anchoredPosition, Vector2 pointerDelta, float canvasScale)
        {
            float scale = canvasScale <= 0f ? 1f : canvasScale;
            return anchoredPosition + pointerDelta / scale;
        }
    }
}
