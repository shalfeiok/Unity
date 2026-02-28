using System.Collections.Generic;
using Game.Domain.Poe.Gems;

namespace Game.Presentation.UI.Windows.Skills
{
    public sealed class SkillsPanelService
    {
        public SkillsPanelState BuildFromCompiledSkills(IReadOnlyList<CompiledSkill> compiledSkills)
        {
            var state = new SkillsPanelState();

            for (int i = 0; i < compiledSkills.Count; i++)
            {
                var compiled = compiledSkills[i];
                var entry = new SkillsPanelEntry
                {
                    SkillId = compiled.SkillId
                };

                for (int s = 0; s < compiled.AppliedSupportGemIds.Count; s++)
                    entry.AppliedSupports.Add(compiled.AppliedSupportGemIds[s]);

                for (int r = 0; r < compiled.RejectedSupports.Count; r++)
                    entry.RejectedSupportReasons.Add(compiled.RejectedSupports[r].Reason);

                state.Entries.Add(entry);
            }

            return state;
        }
    }
}
