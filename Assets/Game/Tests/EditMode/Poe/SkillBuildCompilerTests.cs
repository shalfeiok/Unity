using System.Collections.Generic;
using Game.Domain.Poe.Gems;
using Game.Domain.Poe.Sockets;
using NUnit.Framework;

namespace Game.Tests.EditMode.Poe
{
    public sealed class SkillBuildCompilerTests
    {
        [Test]
        public void Supports_ApplyOnlyWhenInSameLinkGroup()
        {
            var sockets = new SocketModel(3);
            sockets.GemIds[0] = "skill_fireball";
            sockets.GemIds[1] = "support_added_damage";
            sockets.GemIds[2] = "support_chain";
            sockets.Links.Add(new LinkGroup { Indices = { 0, 1 } });
            sockets.Links.Add(new LinkGroup { Indices = { 2 } });

            var skills = new Dictionary<string, SkillGemDefinition>
            {
                ["skill_fireball"] = new() { Id = "skill_fireball", SkillId = "Fireball", Tags = new[] { "spell", "projectile" } }
            };

            var supports = new Dictionary<string, SupportGemDefinition>
            {
                ["support_added_damage"] = new() { Id = "support_added_damage", RequiredTags = new[] { "spell" }, Order = 2 },
                ["support_chain"] = new() { Id = "support_chain", RequiredTags = new[] { "projectile" }, Order = 1 }
            };

            var compiler = new SkillBuildCompiler();
            var result = compiler.Build(sockets, skills, supports);

            CollectionAssert.AreEqual(new[] { "support_added_damage" }, result.AppliedSupportGemIds);
            Assert.AreEqual(1, result.RejectedSupports.Count);
            Assert.IsNotEmpty(result.RejectedSupports[0].Reason);
        }

        [Test]
        public void RejectedSupports_ContainReason_AndOrderingIsStable()
        {
            var sockets = new SocketModel(3);
            sockets.GemIds[0] = "skill_fireball";
            sockets.GemIds[1] = "z_support_bad_tag";
            sockets.GemIds[2] = "a_support_not_linked";
            sockets.Links.Add(new LinkGroup { Indices = { 0, 1 } });
            sockets.Links.Add(new LinkGroup { Indices = { 2 } });

            var skills = new Dictionary<string, SkillGemDefinition>
            {
                ["skill_fireball"] = new() { Id = "skill_fireball", SkillId = "Fireball", Tags = new[] { "spell" } }
            };

            var supports = new Dictionary<string, SupportGemDefinition>
            {
                ["z_support_bad_tag"] = new() { Id = "z_support_bad_tag", RequiredTags = new[] { "melee" }, Order = 5 },
                ["a_support_not_linked"] = new() { Id = "a_support_not_linked", RequiredTags = new[] { "spell" }, Order = 1 }
            };

            var result = new SkillBuildCompiler().Build(sockets, skills, supports);

            Assert.AreEqual(0, result.AppliedSupportGemIds.Count);
            Assert.AreEqual(2, result.RejectedSupports.Count);
            Assert.AreEqual("a_support_not_linked", result.RejectedSupports[0].GemId);
            Assert.AreEqual("z_support_bad_tag", result.RejectedSupports[1].GemId);
            Assert.IsNotEmpty(result.RejectedSupports[0].Reason);
            Assert.IsNotEmpty(result.RejectedSupports[1].Reason);
        }

        [Test]
        public void BuildDetailed_ProvidesExplainEntries_WithRequiredAndMissingTags()
        {
            var sockets = new SocketModel(3);
            sockets.GemIds[0] = "skill_fireball";
            sockets.GemIds[1] = "support_spell";
            sockets.GemIds[2] = "support_melee";
            sockets.Links.Add(new LinkGroup { Indices = { 0, 1, 2 } });

            var skills = new Dictionary<string, SkillGemDefinition>
            {
                ["skill_fireball"] = new() { Id = "skill_fireball", SkillId = "Fireball", Tags = new[] { "spell" } }
            };
            var supports = new Dictionary<string, SupportGemDefinition>
            {
                ["support_spell"] = new() { Id = "support_spell", RequiredTags = new[] { "spell" }, Order = 2 },
                ["support_melee"] = new() { Id = "support_melee", RequiredTags = new[] { "melee" }, Order = 1 }
            };

            var result = new SkillBuildCompiler().BuildDetailed(sockets, skills, supports);

            Assert.AreEqual(2, result.ExplainEntries.Count);
            Assert.AreEqual("support_melee", result.ExplainEntries[0].GemId);
            Assert.False(result.ExplainEntries[0].IsApplied);
            Assert.AreEqual(SupportRejectReason.MissingRequiredTags, result.ExplainEntries[0].RejectReason);
            CollectionAssert.AreEqual(new[] { "melee" }, result.ExplainEntries[0].MissingTags);

            Assert.AreEqual("support_spell", result.ExplainEntries[1].GemId);
            Assert.True(result.ExplainEntries[1].IsApplied);
            Assert.AreEqual(SupportRejectReason.None, result.ExplainEntries[1].RejectReason);
        }
    }
}
