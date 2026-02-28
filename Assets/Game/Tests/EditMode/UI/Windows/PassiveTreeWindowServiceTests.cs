using Game.Domain.Poe.Passives;
using Game.Presentation.UI.Windows.PassiveTree;
using NUnit.Framework;
using UnityEngine;

namespace Game.Tests.EditMode.UI.Windows
{
    public sealed class PassiveTreeWindowServiceTests
    {
        [Test]
        public void Zoom_ClampsToRange()
        {
            var state = new PassiveTreeWindowState();
            var service = new PassiveTreeWindowService();

            service.ApplyZoom(state, -5f);
            Assert.AreEqual(0.5f, state.Zoom);

            service.ApplyZoom(state, 10f);
            Assert.AreEqual(2.5f, state.Zoom);
        }

        [Test]
        public void Pan_AccumulatesDelta()
        {
            var state = new PassiveTreeWindowState();
            var service = new PassiveTreeWindowService();

            service.ApplyPan(state, new Vector2(3, -2));
            service.ApplyPan(state, new Vector2(-1, 5));

            Assert.AreEqual(new Vector2(2, 3), state.Pan);
        }

        [Test]
        public void Search_HighlightsMatchingNodes_AndPreviewSelectsExistingNode()
        {
            var tree = new PassiveTreeDefinition();
            tree.Nodes.Add(new PassiveNodeDefinition { Id = "life_small", Name = "Small Life", Tags = new System.Collections.Generic.List<string> { "life" } });
            tree.Nodes.Add(new PassiveNodeDefinition { Id = "life_big", Name = "Big Life", Tags = new System.Collections.Generic.List<string> { "defense" } });
            tree.Nodes.Add(new PassiveNodeDefinition { Id = "crit_small", Name = "Critical Chance", Tags = new System.Collections.Generic.List<string> { "critical" } });

            var state = new PassiveTreeWindowState();
            var service = new PassiveTreeWindowService();

            var result = service.Search(state, tree, "critical");

            Assert.AreEqual(1, result.Count);
            Assert.True(state.HighlightedNodeIds.Contains("crit_small"));

            var lifeResult = service.Search(state, tree, "life");
            Assert.AreEqual(2, lifeResult.Count);
            Assert.True(state.HighlightedNodeIds.Contains("life_small"));
            Assert.True(service.SetPreviewNode(state, tree, "life_big"));
            Assert.AreEqual("life_big", state.PreviewNodeId);
            Assert.False(service.SetPreviewNode(state, tree, "unknown"));
        }
    }
}
