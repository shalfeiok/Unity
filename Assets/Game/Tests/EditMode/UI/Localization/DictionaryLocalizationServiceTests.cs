using Game.Presentation.UI.Localization;
using NUnit.Framework;

namespace Game.Tests.EditMode.UI.Localization
{
    public sealed class DictionaryLocalizationServiceTests
    {
        [Test]
        public void Translate_ReturnsRussianValue_WhenKeyExists()
        {
            var service = new DictionaryLocalizationService(RussianUiStrings.BuildDefault());

            Assert.AreEqual("Редкий", service.Translate("rarity.rare"));
            Assert.AreEqual("missing.key", service.Translate("missing.key"));
        }

        [Test]
        public void TranslateFormat_AppliesArguments()
        {
            var service = new DictionaryLocalizationService(RussianUiStrings.BuildDefault());

            Assert.AreEqual("Качество: +12%", service.TranslateFormat("tooltip.item.quality", 12));
        }
    }
}
