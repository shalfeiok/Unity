# UI (UGUI): окна, hotbar, tooltips, debug overlay)


## Принципы
- UI = View (MonoBehaviour) + Presenter/ViewModel (pure C#).
- View НЕ вычисляет бизнес-логику, только биндинг.
- Все окна подключаются через `UIWindowManager`.
- Вся типографика/tooltips должны уметь “breakdown” источников модов (items/buffs/aspects).

## Окна и функции
### 1) Hotbar (10 слотов)
- View:
  - отображение иконки умения, кд, подсветка доступности
  - обработка клика по слоту
- Presenter:
  - `BindSlots(HotbarModel model)`
  - `OnSlotPressed(int index)`
  - `OnSlotClicked(int index)`
  - обновления кд таймеров: без аллокаций (кэш Text refs)
- Связь:
  - hotbar → `PlayerCommandController.RequestCast(slot)`

### 2) InventoryWindow (I)
- Функции:
  - grid отображение предметов
  - drag&drop (опционально), экип/перемещение
  - tooltip на hover
- Presenter:
  - `LoadInventory(InventoryModel)`
  - `OnItemClicked(itemId)` / `OnItemRightClicked(itemId)` (equip)
  - `OnItemHovered(itemId)` (tooltip)
  - `OnDropItem(itemId)` (опционально)

### 3) Character/StatusWindow (C)
- Функции:
  - отображение primary/offense/defense/utility
  - breakdown: “base + item mods + buffs”
- Presenter:
  - `GetStatRows()` → список строк статов (id, name, value, breakdown)

### 4) SkillBookWindow (K)
- Функции:
  - список умений, фильтры по тегам, поиск
  - назначение на hotbar
- Presenter:
  - `ListSkills(filter)`
  - `AssignToHotbar(skillId, slotIndex)`

### 5) CraftingWindow
- Функции:
  - выбор компонентов (Core/Element/Modifier/Catalyst)
  - preview budgets (Power/Complexity/VisualCost)
  - reroll (deterministic варианты)
  - craft → добавление результата в SkillBook/Inventory
- Presenter:
  - `SetSeed(int seed)` (dev)
  - `SelectComponent(ComponentType, componentId)`
  - `Preview()` → `CraftPreviewModel`
  - `Reroll()` → новый preview (seed step)
  - `Craft()` → результат

### 6) Target UI
- Функции:
  - имя/HP/статусы цели
  - индикатор дистанции/вне range (опционально)

### 7) Debug Overlay (dev)
- Функции:
  - отображение:
    - активных статусов,
    - последних combat events,
    - last executed nodes,
    - proc budget counters,
    - seed текущего мира/крафта.
- Требования:
  - выключаемое, не влияет на релиз.

## UI Input blocking
- В проекте обязателен `InputRouter` приоритетов:
  - UI → блок world input
  - drag → блок single-click (threshold)
