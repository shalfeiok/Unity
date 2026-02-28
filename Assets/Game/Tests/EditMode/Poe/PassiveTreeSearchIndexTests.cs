using System.Collections.Generic;
using Game.Domain.Poe.Passives;
using NUnit.Framework;

namespace Game.Tests.EditMode.Poe
{
    public sealed class PassiveTreeSearchIndexTests
    {
        [Test]
        public void Search_FindsByIdNameAndTags_WithoutDuplicates()
        {
            var tree = new PassiveTreeDefinition();
            tree.Nodes.Add(new PassiveNodeDefinition
            {
                Id = "life_small",
                Name = "Heart of Life",
                Tags = new List<string> { "life", "defense" }
            });
            tree.Nodes.Add(new PassiveNodeDefinition
            {
                Id = "spell_crit",
                Name = "Arcane Focus",
                Tags = new List<string> { "spell", "critical" }
            });

            var index = new PassiveTreeSearchIndex(tree);

            var byId = index.Search("life");
            var byName = index.Search("arcane");
            var byTag = index.Search("critical");

            CollectionAssert.AreEquivalent(new[] { "life_small" }, byId);
            CollectionAssert.AreEquivalent(new[] { "spell_crit" }, byName);
            CollectionAssert.AreEquivalent(new[] { "spell_crit" }, byTag);
        }
    }
}
