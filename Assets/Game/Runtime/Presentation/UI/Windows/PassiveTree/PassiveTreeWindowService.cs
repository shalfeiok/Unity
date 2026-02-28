using System;
using System.Collections.Generic;
using Game.Domain.Poe.Passives;
using UnityEngine;

namespace Game.Presentation.UI.Windows.PassiveTree
{
    public sealed class PassiveTreeWindowService
    {
        public void ApplyZoom(PassiveTreeWindowState state, float delta, float min = 0.5f, float max = 2.5f)
        {
            state.Zoom = Mathf.Clamp(state.Zoom + delta, min, max);
        }

        public void ApplyPan(PassiveTreeWindowState state, Vector2 delta)
        {
            state.Pan += delta;
        }

        public IReadOnlyList<string> Search(PassiveTreeWindowState state, PassiveTreeDefinition tree, string query)
        {
            state.SearchQuery = query ?? string.Empty;
            state.HighlightedNodeIds.Clear();

            if (string.IsNullOrWhiteSpace(state.SearchQuery))
                return Array.Empty<string>();

            var result = new List<string>();
            for (int i = 0; i < tree.Nodes.Count; i++)
            {
                var id = tree.Nodes[i].Id;
                if (id != null && id.IndexOf(state.SearchQuery, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    state.HighlightedNodeIds.Add(id);
                    result.Add(id);
                }
            }

            return result;
        }

        public bool SetPreviewNode(PassiveTreeWindowState state, PassiveTreeDefinition tree, string nodeId)
        {
            for (int i = 0; i < tree.Nodes.Count; i++)
            {
                if (tree.Nodes[i].Id == nodeId)
                {
                    state.PreviewNodeId = nodeId;
                    return true;
                }
            }

            return false;
        }
    }
}
