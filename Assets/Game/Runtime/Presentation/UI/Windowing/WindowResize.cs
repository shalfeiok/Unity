using UnityEngine;

namespace Game.Presentation.UI.Windowing
{
    public static class WindowResize
    {
        public static Vector2 Resize(Vector2 currentSize, Vector2 delta, Vector2 minSize, Vector2 maxSize)
        {
            var target = currentSize + delta;
            target.x = Mathf.Clamp(target.x, minSize.x, maxSize.x);
            target.y = Mathf.Clamp(target.y, minSize.y, maxSize.y);
            return target;
        }
    }
}
