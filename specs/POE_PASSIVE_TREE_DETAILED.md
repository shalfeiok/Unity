# POE_PASSIVE_TREE_DETAILED.md (атомарная спецификация)

Дата: 2026-02-27

Цель: реализовать большое дерево пассивов PoE-стиля: граф, поиск, zoom/pan, preview, allocation, respec.

---

## 1) Data model
### 1.1 PassiveTreeDefinition (authoring)
- `TreeId`
- `Nodes[]`:
  - `NodeId`
  - `NameKey`
  - `NodeType`: Small/Notable/Keystone/JewelSocket
  - `Position` (x,y) для UI
  - `Effects[]` (Modifier v2: scope tags + buckets)
  - `Tags[]` (например: defense, offense, fire, minion)
- `Edges[]`: пары NodeId (неориентированный)
- `StartNodesByClass`: classTag → nodeId
- `Rules`:
  - max allocated (optional)
  - keystone uniqueness (обычно unique by NodeId)

### 1.2 Runtime state (CharacterProfile)
- `AllocatedNodes` (BitSet recommended)
- `AvailablePoints`
- `RespecPoints`
- `PendingPreview` (temporary set during planning)
- `UndoStack` (optional)

---

## 2) Graph rules
- Node allocatable iff:
  - node is start node OR
  - имеет соседа в AllocatedNodes
- Allocation consumes 1 point (или `Cost` per node).
- Respec:
  - нельзя ресpecнуть node, если это разорвёт связность от стартового узла (или разрешить, но тогда нужно “auto-unallocate” — выберите один вариант).
Рекомендовано (лучшее): **запрет** на respec, который делает граф недостижимым от стартового узла.

---

## 3) Effects application
- Каждая нода даёт набор Modifier v2.
- При изменении AllocatedNodes:
  - пересобрать агрегированный ModifierSet для персонажа (cache + diff).
- Для tooltip:
  - provenance: NodeId → modifiers.

---

## 4) UI (Window)
### 4.1 Навигация
- zoom (колесо)
- pan (drag background)
- миникарта дерева (optional MVP)
- кнопка “центрировать на старте”

### 4.2 Интеракции
- Hover node → tooltip (name + эффекты + тэги)
- Click node:
  - если allocatable и есть points → показать preview (изменение ключевых статов), затем confirm
- Search:
  - поле ввода
  - результаты списком
  - click result → фокус на node, подсветка пути от ближайшей allocated.

### 4.3 Preview
- До подтверждения: показывать “Δ статы” (DPS/HP/Resists/etc.) в правой панели.
- Preview строится через временный ModifierSet = current + node effects.

### 4.4 Respec
- режим respec (toggle) или контекстное меню
- списание RespecPoints
- валидатор связности

---

## 5) Валидаторы данных (Editor + CI)
- Graph connectivity from start nodes (все nodes должны быть достижимы от хотя бы одного старта)
- No cycles allowed? (циклы допустимы, главное — корректность)
- No duplicate NodeId
- No invalid edges
- Positions in reasonable bounds (не NaN)

---

## 6) Тесты
### EditMode
- alloc rules
- respec connectivity validator
- deterministic aggregated modifiers
- search indexing (node by name/tag)

### PlayMode
- open window, zoom/pan, allocate node updates stats UI.
