using System.Collections.Generic;
using UnityEngine;

namespace Game.Presentation.UI.Windows.PassiveTree
{
    public sealed class PassiveTreeWindowState
    {
        public float Zoom = 1f;
        public Vector2 Pan = Vector2.zero;
        public string SearchQuery = string.Empty;
        public HashSet<string> HighlightedNodeIds = new();
        public string PreviewNodeId = string.Empty;
    }
}
