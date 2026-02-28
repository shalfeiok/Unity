using System.Collections.Generic;
using Game.Domain.Poe.Items;
using Game.Presentation.UI.Localization;
using Game.Presentation.UI.Tooltips;
using NUnit.Framework;

namespace Game.Tests.EditMode.UI.Tooltips
{
    public sealed class ItemTooltipBuilderTests
    {
        [Test]
        public void Build_CreatesRussianTooltipSections_WithItemData()
        {
            var item = new GeneratedPoeItem(
                new ItemBaseDefinition { Id = "sword_base" },
                68,
                new List<GeneratedPoeMod>
                {
                    new(new ModDefinition { Id = "p_phys" }, 42f)
                },
                new List<GeneratedPoeMod>
                {
                    new(new ModDefinition { Id = "imp_crit" }, 5f)
                },
                quality: 12,
                socketColors: new[] { SocketColor.Red, SocketColor.Green },
                linkGroups: new[] { new[] { 0, 1 } },
                isCorrupted: false);

            var builder = new ItemTooltipBuilder(new DictionaryLocalizationService(RussianUiStrings.BuildDefault()));
            var tooltip = builder.Build(item);

            Assert.AreEqual("Предмет: sword_base", tooltip.Title);
            Assert.AreEqual("Уровень предмета: 68", tooltip.Subtitle);
            Assert.AreEqual("Имплиситы", tooltip.Sections[1].Header);
            Assert.AreEqual("Модификаторы", tooltip.Sections[2].Header);
            Assert.AreEqual("Связи", tooltip.Sections[3].Header);
            Assert.AreEqual("0-1", tooltip.Sections[3].Lines[0]);
        }
    }
}
