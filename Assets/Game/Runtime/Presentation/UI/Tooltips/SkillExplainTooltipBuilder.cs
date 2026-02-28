using Game.Domain.Poe.Gems;
using Game.Presentation.UI.Localization;

namespace Game.Presentation.UI.Tooltips
{
    public sealed class SkillExplainTooltipBuilder
    {
        private readonly ILocalizationService _loc;

        public SkillExplainTooltipBuilder(ILocalizationService loc)
        {
            _loc = loc;
        }

        public TooltipModel Build(CompiledSkill compiled)
        {
            var model = new TooltipModel
            {
                Title = _loc.TranslateFormat("tooltip.skill.title", string.IsNullOrWhiteSpace(compiled.SkillId) ? _loc.Translate("tooltip.empty") : compiled.SkillId),
                Subtitle = _loc.TranslateFormat("tooltip.skill.applied_count", compiled.AppliedSupportGemIds.Count)
            };

            var applied = new TooltipSection { Header = _loc.Translate("tooltip.section.applied_supports") };
            for (int i = 0; i < compiled.ExplainEntries.Count; i++)
            {
                var entry = compiled.ExplainEntries[i];
                if (!entry.IsApplied)
                    continue;

                applied.Lines.Add(_loc.TranslateFormat("tooltip.skill.applied_line", entry.GemId, entry.Order));
            }
            if (applied.Lines.Count == 0)
                applied.Lines.Add(_loc.Translate("tooltip.empty"));
            model.Sections.Add(applied);

            var rejected = new TooltipSection { Header = _loc.Translate("tooltip.section.rejected_supports") };
            for (int i = 0; i < compiled.ExplainEntries.Count; i++)
            {
                var entry = compiled.ExplainEntries[i];
                if (entry.IsApplied)
                    continue;

                var missing = entry.MissingTags.Count == 0 ? _loc.Translate("tooltip.empty") : string.Join(", ", entry.MissingTags);
                rejected.Lines.Add(_loc.TranslateFormat("tooltip.skill.rejected_line", entry.GemId, entry.ReasonText, missing));
            }
            if (rejected.Lines.Count == 0)
                rejected.Lines.Add(_loc.Translate("tooltip.empty"));
            model.Sections.Add(rejected);

            return model;
        }
    }
}
