# Интеракции и данные UI (Lineage II + Diablo III ориентиры)

Дата: 2026-02-27

## 1) Единая модель взаимодействия
- Hover → подсветка/tooltip/обновление target frame
- ЛКМ: движение/атака/выбор цели (ARPG режим)
- ПКМ: контекстное действие
- Esc: закрыть верхнее окно / отмена
- Enter: фокус чат
- Alt: показать названия предметов (loot labels)
- I/C/K/M/J: ключевые окна

## 2) Drag&Drop матрица
- Инвентарь ↔ Экипировка
- Инвентарь ↔ Stash
- Инвентарь ↔ Торговля (trade)
- Инвентарь ↔ NPC магазин (sell)
- Инвентарь ↔ Крафт (salvage/upgrade)
- Skills/Items ↔ Hotbar (assign)
- split/merge/swap

Каждая операция:
- валидация,
- cursor feedback,
- подтверждение опасных действий.

## 3) Tooltips (4 режима)
- normal / pinned / compare / advanced (ALT)

## 4) Подвижный UI
- drag title bar
- dock/snap
- сохранение layout
- масштаб UI и “сброс UI”

## 5) Данные UI (ViewModels + события)
UI подписывается на:
- PlayerStatsChanged, TargetChanged, InventoryChanged,
- LootSpawned/LootDespawned, QuestUpdated, ChatMessageReceived

## 6) Acceptance criteria
- действия не конфликтуют (чат vs хоткеи),
- layout восстанавливается,
- drag&drop и tooltips работают.
