using UnityEngine;

namespace Game.Presentation.UI.Windowing
{
    public static class WindowDockSnap
    {
        public static Vector2 SnapToBounds(Vector2 pos, Rect bounds, float threshold)
        {
            float x = pos.x;
            float y = pos.y;

            if (Mathf.Abs(pos.x - bounds.xMin) <= threshold) x = bounds.xMin;
            if (Mathf.Abs(pos.x - bounds.xMax) <= threshold) x = bounds.xMax;
            if (Mathf.Abs(pos.y - bounds.yMin) <= threshold) y = bounds.yMin;
            if (Mathf.Abs(pos.y - bounds.yMax) <= threshold) y = bounds.yMax;

            return new Vector2(x, y);
        }
    }
}
