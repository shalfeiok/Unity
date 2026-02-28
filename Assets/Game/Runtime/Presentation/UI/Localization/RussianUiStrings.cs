using System.Collections.Generic;

namespace Game.Presentation.UI.Localization
{
    public static class RussianUiStrings
    {
        public static IReadOnlyDictionary<string, string> BuildDefault()
        {
            return new Dictionary<string, string>
            {
                ["tooltip.item.title"] = "Предмет: {0}",
                ["tooltip.item.ilvl"] = "Уровень предмета: {0}",
                ["tooltip.item.quality"] = "Качество: +{0}%",
                ["tooltip.item.sockets"] = "Сокеты: {0}",
                ["tooltip.section.implicits"] = "Имплиситы",
                ["tooltip.section.modifiers"] = "Модификаторы",
                ["tooltip.section.links"] = "Связи",
                ["tooltip.empty"] = "Нет данных",
                ["rarity.common"] = "Обычный",
                ["rarity.magic"] = "Магический",
                ["rarity.rare"] = "Редкий",
                ["rarity.legendary"] = "Легендарный",
                ["ui.error.precondition"] = "Действие недоступно: условия не выполнены"
            };
        }
    }
}
