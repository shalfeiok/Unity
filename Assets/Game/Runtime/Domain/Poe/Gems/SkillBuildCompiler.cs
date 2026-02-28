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
            return BuildDetailed(sockets, skills, supports);
        }

        public CompiledSkill BuildDetailed(
            SocketModel sockets,
            IReadOnlyDictionary<string, SkillGemDefinition> skills,
            IReadOnlyDictionary<string, SupportGemDefinition> supports)
        {
            var applied = new List<string>();
            var rejected = new List<RejectedSupport>();
            var explain = new List<SupportExplainEntry>();
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
                return new CompiledSkill(string.Empty, applied, rejected, explain);

            var appliedSortKeys = new List<(int order, string gemId)>();
            var rejectedSortKeys = new List<(int order, string gemId, string reason)>();

            for (int i = 0; i < sockets.GemIds.Length; i++)
            {
                var gemId = sockets.GemIds[i];
                if (string.IsNullOrEmpty(gemId)) continue;
                if (!supports.TryGetValue(gemId, out var support)) continue;

                bool linked = IsLinkedToMainSkill(i, sockets, skills);
                var missingTags = GetMissingTags(tags, support.RequiredTags);

                if (!linked)
                {
                    const string reason = "Support gem is not linked to the active skill.";
                    rejectedSortKeys.Add((support.Order, gemId, reason));
                    explain.Add(new SupportExplainEntry(gemId, support.Order, false, SupportRejectReason.NotLinkedToSkill, reason, support.RequiredTags ?? Array.Empty<string>(), missingTags));
                    continue;
                }

                if (missingTags.Count > 0)
                {
                    const string reason = "Support tags are incompatible with skill.";
                    rejectedSortKeys.Add((support.Order, gemId, reason));
                    explain.Add(new SupportExplainEntry(gemId, support.Order, false, SupportRejectReason.MissingRequiredTags, reason, support.RequiredTags ?? Array.Empty<string>(), missingTags));
                    continue;
                }

                appliedSortKeys.Add((support.Order, gemId));
                explain.Add(new SupportExplainEntry(gemId, support.Order, true, SupportRejectReason.None, "Applied", support.RequiredTags ?? Array.Empty<string>(), Array.Empty<string>()));
            }

            appliedSortKeys.Sort(static (a, b) => CompareByOrderThenGemId(a.order, a.gemId, b.order, b.gemId));
            rejectedSortKeys.Sort(static (a, b) => CompareByOrderThenGemId(a.order, a.gemId, b.order, b.gemId));
            explain.Sort(static (a, b) => CompareByOrderThenGemId(a.Order, a.GemId, b.Order, b.GemId));

            for (int i = 0; i < appliedSortKeys.Count; i++)
                applied.Add(appliedSortKeys[i].gemId);

            for (int i = 0; i < rejectedSortKeys.Count; i++)
                rejected.Add(new RejectedSupport(rejectedSortKeys[i].gemId, rejectedSortKeys[i].reason));

            return new CompiledSkill(skillId, applied, rejected, explain);
        }

        private static int CompareByOrderThenGemId(int orderA, string gemA, int orderB, string gemB)
        {
            int cmp = orderA.CompareTo(orderB);
            if (cmp != 0)
                return cmp;

            return string.CompareOrdinal(gemA, gemB);
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

        private static List<string> GetMissingTags(string[] skillTags, string[] requiredTags)
        {
            var missing = new List<string>();
            if (requiredTags == null || requiredTags.Length == 0)
                return missing;

            if (skillTags == null || skillTags.Length == 0)
            {
                for (int i = 0; i < requiredTags.Length; i++)
                    missing.Add(requiredTags[i]);
                return missing;
            }

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

                if (!found)
                    missing.Add(requiredTags[i]);
            }

            return missing;
        }
    }

    public enum SupportRejectReason
    {
        None = 0,
        NotLinkedToSkill = 1,
        MissingRequiredTags = 2
    }

    public readonly struct SupportExplainEntry
    {
        public SupportExplainEntry(
            string gemId,
            int order,
            bool isApplied,
            SupportRejectReason rejectReason,
            string reasonText,
            IReadOnlyList<string> requiredTags,
            IReadOnlyList<string> missingTags)
        {
            GemId = gemId;
            Order = order;
            IsApplied = isApplied;
            RejectReason = rejectReason;
            ReasonText = reasonText;
            RequiredTags = requiredTags;
            MissingTags = missingTags;
        }

        public string GemId { get; }
        public int Order { get; }
        public bool IsApplied { get; }
        public SupportRejectReason RejectReason { get; }
        public string ReasonText { get; }
        public IReadOnlyList<string> RequiredTags { get; }
        public IReadOnlyList<string> MissingTags { get; }
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
            : this(skillId, appliedSupportGemIds, rejectedSupports, Array.Empty<SupportExplainEntry>())
        {
        }

        public CompiledSkill(
            string skillId,
            IReadOnlyList<string> appliedSupportGemIds,
            IReadOnlyList<RejectedSupport> rejectedSupports,
            IReadOnlyList<SupportExplainEntry> explainEntries)
        {
            SkillId = skillId;
            AppliedSupportGemIds = appliedSupportGemIds;
            RejectedSupports = rejectedSupports;
            ExplainEntries = explainEntries;
        }

        public string SkillId { get; }
        public IReadOnlyList<string> AppliedSupportGemIds { get; }
        public IReadOnlyList<RejectedSupport> RejectedSupports { get; }
        public IReadOnlyList<SupportExplainEntry> ExplainEntries { get; }
    }
}
