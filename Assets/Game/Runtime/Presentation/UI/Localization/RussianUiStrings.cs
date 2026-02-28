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
                ["tooltip.skill.title"] = "Навык: {0}",
                ["tooltip.skill.applied_count"] = "Применённых поддержек: {0}",
                ["tooltip.section.applied_supports"] = "Применённые поддержки",
                ["tooltip.section.rejected_supports"] = "Отклонённые поддержки",
                ["tooltip.skill.applied_line"] = "{0} (приоритет {1})",
                ["tooltip.skill.rejected_line"] = "{0}: {1}; отсутствуют теги: {2}",
                ["ui.error.precondition"] = "Действие недоступно: условия не выполнены",
                ["craft.apply.success"] = "Крафт успешно применён: {0}",
                ["event.gem_inserted"] = "Самоцвет вставлен",
                ["event.gem_removed"] = "Самоцвет извлечён",
                ["event.passive_allocated"] = "Пассивный узел выделен",
                ["event.passive_refunded"] = "Пассивный узел возвращён",
                ["event.currency_applied"] = "Валюта применена к предмету",
                ["event.currency_applied_detailed"] = "Валюта {0} применена к предмету {1}",
                ["event.flask_used"] = "Фласка использована",
                ["event.flask_used_detailed"] = "Фласка использована: {0}",
                ["event.hotbar_assigned"] = "Навык назначен на панель",
                ["event.hotbar_unassigned"] = "Навык снят с панели",
                ["event.loot_picked_up"] = "Предмет поднят",
                ["event.loot_picked_up_detailed"] = "Поднят предмет {0} x{1} ({2})",
                ["event.unknown"] = "Игровое событие"
            };
        }
    }
}
