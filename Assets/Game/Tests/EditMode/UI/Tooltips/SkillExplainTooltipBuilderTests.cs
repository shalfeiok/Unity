using System.Collections.Generic;
using Game.Domain.Poe.Gems;
using Game.Presentation.UI.Localization;
using Game.Presentation.UI.Tooltips;
using NUnit.Framework;

namespace Game.Tests.EditMode.UI.Tooltips
{
    public sealed class SkillExplainTooltipBuilderTests
    {
        [Test]
        public void Build_CreatesRussianExplainTooltip_WithAppliedAndRejectedSections()
        {
            var explain = new List<SupportExplainEntry>
            {
                new("support_added", 1, true, SupportRejectReason.None, "Applied", new[] { "spell" }, new string[0]),
                new("support_melee", 2, false, SupportRejectReason.MissingRequiredTags, "Support tags are incompatible with skill.", new[] { "melee" }, new[] { "melee" })
            };

            var compiled = new CompiledSkill(
                "Fireball",
                new List<string> { "support_added" },
                new List<RejectedSupport> { new("support_melee", "Support tags are incompatible with skill.") },
                explain);

            var builder = new SkillExplainTooltipBuilder(new DictionaryLocalizationService(RussianUiStrings.BuildDefault()));
            var tooltip = builder.Build(compiled);

            Assert.AreEqual("Навык: Fireball", tooltip.Title);
            Assert.AreEqual("Применённых поддержек: 1", tooltip.Subtitle);
            Assert.AreEqual("Применённые поддержки", tooltip.Sections[0].Header);
            Assert.AreEqual("Отклонённые поддержки", tooltip.Sections[1].Header);
            StringAssert.Contains("support_melee", tooltip.Sections[1].Lines[0]);
            StringAssert.Contains("отсутствуют теги", tooltip.Sections[1].Lines[0]);
        }
    }
}
