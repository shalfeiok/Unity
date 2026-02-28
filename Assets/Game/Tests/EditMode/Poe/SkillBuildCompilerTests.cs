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
                ["support_added_damage"] = new() { Id = "support_added_damage", RequiredTags = new[] { "spell" } },
                ["support_chain"] = new() { Id = "support_chain", RequiredTags = new[] { "projectile" } }
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
                ["z_support_bad_tag"] = new() { Id = "z_support_bad_tag", RequiredTags = new[] { "melee" } },
                ["a_support_not_linked"] = new() { Id = "a_support_not_linked", RequiredTags = new[] { "spell" } }
            };

            var result = new SkillBuildCompiler().Build(sockets, skills, supports);

            Assert.AreEqual(0, result.AppliedSupportGemIds.Count);
            Assert.AreEqual(2, result.RejectedSupports.Count);
            Assert.AreEqual("a_support_not_linked", result.RejectedSupports[0].GemId);
            Assert.AreEqual("z_support_bad_tag", result.RejectedSupports[1].GemId);
            Assert.IsNotEmpty(result.RejectedSupports[0].Reason);
            Assert.IsNotEmpty(result.RejectedSupports[1].Reason);
        }
    }
}
