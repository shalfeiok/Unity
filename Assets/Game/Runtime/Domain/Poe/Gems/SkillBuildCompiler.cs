using System;
using System.Collections.Generic;
using Game.Domain.Poe.Sockets;

namespace Game.Domain.Poe.Gems
{
    public sealed class SkillBuildCompiler
    {
        public CompiledSkill Build(
            SocketModel sockets,
            IReadOnlyDictionary<string, SkillGemDefinition> skills,
            IReadOnlyDictionary<string, SupportGemDefinition> supports)
        {
            var applied = new List<string>();
            var rejected = new List<RejectedSupport>();
            string skillId = string.Empty;
            var tags = Array.Empty<string>();

            for (int i = 0; i < sockets.GemIds.Length; i++)
            {
                var gemId = sockets.GemIds[i];
                if (string.IsNullOrEmpty(gemId)) continue;
                if (skills.TryGetValue(gemId, out var skill))
                {
                    skillId = skill.SkillId;
                    tags = skill.Tags ?? Array.Empty<string>();
                    break;
                }
            }

            if (string.IsNullOrEmpty(skillId))
                return new CompiledSkill(string.Empty, applied, rejected);

            for (int i = 0; i < sockets.GemIds.Length; i++)
            {
                var gemId = sockets.GemIds[i];
                if (string.IsNullOrEmpty(gemId)) continue;
                if (!supports.TryGetValue(gemId, out var support)) continue;

                if (!IsLinkedToMainSkill(i, sockets, skills))
                {
                    rejected.Add(new RejectedSupport(gemId, "Support gem is not linked to the active skill."));
                    continue;
                }

                if (!MatchesTags(tags, support.RequiredTags))
                {
                    rejected.Add(new RejectedSupport(gemId, "Support tags are incompatible with skill."));
                    continue;
                }

                applied.Add(gemId);
            }

            applied.Sort(StringComparer.Ordinal);
            rejected.Sort(static (a, b) => string.CompareOrdinal(a.GemId, b.GemId));
            return new CompiledSkill(skillId, applied, rejected);
        }

        private static bool IsLinkedToMainSkill(int supportIndex, SocketModel sockets, IReadOnlyDictionary<string, SkillGemDefinition> skills)
        {
            foreach (var group in sockets.Links)
            {
                if (!group.Contains(supportIndex)) continue;

                foreach (var idx in group.Indices)
                {
                    if (idx < 0 || idx >= sockets.GemIds.Length) continue;
                    var gem = sockets.GemIds[idx];
                    if (!string.IsNullOrEmpty(gem) && skills.ContainsKey(gem))
                        return true;
                }
            }

            return false;
        }

        private static bool MatchesTags(string[] skillTags, string[] requiredTags)
        {
            if (requiredTags == null || requiredTags.Length == 0) return true;
            if (skillTags == null || skillTags.Length == 0) return false;

            for (int i = 0; i < requiredTags.Length; i++)
            {
                bool found = false;
                for (int j = 0; j < skillTags.Length; j++)
                {
                    if (string.Equals(requiredTags[i], skillTags[j], StringComparison.OrdinalIgnoreCase))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found) return false;
            }

            return true;
        }
    }

    public readonly struct RejectedSupport
    {
        public RejectedSupport(string gemId, string reason)
        {
            GemId = gemId;
            Reason = reason;
        }

        public string GemId { get; }
        public string Reason { get; }
    }

    public readonly struct CompiledSkill
    {
        public CompiledSkill(string skillId, IReadOnlyList<string> appliedSupportGemIds, IReadOnlyList<RejectedSupport> rejectedSupports)
        {
            SkillId = skillId;
            AppliedSupportGemIds = appliedSupportGemIds;
            RejectedSupports = rejectedSupports;
        }

        public string SkillId { get; }
        public IReadOnlyList<string> AppliedSupportGemIds { get; }
        public IReadOnlyList<RejectedSupport> RejectedSupports { get; }
    }
}
