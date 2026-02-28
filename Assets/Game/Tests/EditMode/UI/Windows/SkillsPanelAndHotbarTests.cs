using System.Collections.Generic;
using Game.Domain.Poe.Gems;
using Game.Presentation.UI.Hud;
using Game.Presentation.UI.Localization;
using Game.Presentation.UI.Tooltips;
using Game.Presentation.UI.Windows.Skills;
using NUnit.Framework;

namespace Game.Tests.EditMode.UI.Windows
{
    public sealed class SkillsPanelAndHotbarTests
    {
        [Test]
        public void SkillsPanel_BuildsEntries_FromCompiledSkills()
        {
            var compiled = new List<CompiledSkill>
            {
                new(
                    "Fireball",
                    new List<string> { "support_added", "support_echo" },
                    new List<RejectedSupport>
                    {
                        new("support_melee", "Support tags are incompatible with skill.")
                    })
            };

            var localizer = new DictionaryLocalizationService(RussianUiStrings.BuildDefault());
            var tooltipBuilder = new SkillExplainTooltipBuilder(localizer);
            var state = new SkillsPanelService(tooltipBuilder).BuildFromCompiledSkills(compiled);

            Assert.AreEqual(1, state.Entries.Count);
            Assert.AreEqual("Fireball", state.Entries[0].SkillId);
            CollectionAssert.AreEqual(new[] { "support_added", "support_echo" }, state.Entries[0].AppliedSupports);
            Assert.AreEqual(1, state.Entries[0].RejectedSupportReasons.Count);
            Assert.NotNull(state.Entries[0].ExplainTooltip);
            StringAssert.Contains("Навык", state.Entries[0].ExplainTooltip.Title);
        }

        [Test]
        public void Hotbar_AssignAndUnassign_Works()
        {
            var hotbar = new HotbarAssignmentService();

            Assert.True(hotbar.Assign(1, "Fireball"));
            Assert.True(hotbar.TryGetAssignedSkill(1, out var skill));
            Assert.AreEqual("Fireball", skill);

            Assert.True(hotbar.Unassign(1));
            Assert.False(hotbar.TryGetAssignedSkill(1, out _));
        }

        [Test]
        public void Hotbar_Assign_RejectsInvalidInput()
        {
            var hotbar = new HotbarAssignmentService();

            Assert.False(hotbar.Assign(-1, "Fireball"));
            Assert.False(hotbar.Assign(11, "Fireball"));
            Assert.False(hotbar.Assign(2, ""));
        }
    }
}
