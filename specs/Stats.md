# Stat System: статы, модификаторы, бакеты, soft caps


## Цели
- Единая математика статов для игрока и NPC.
- Breakdown каждого итогового значения (источники модов).
- Soft caps для “опасных” статов (crit, CDR, resists).
- Временные моды (бафы/ауры) с TTL.

## Модель данных (Domain)
### 1) Идентификаторы
- `StatId` (enum или stable int hash). Требования:
  - стабильность между версиями (лучше int + таблица).
- `TagId` + `TagSet` (см. `Assets/Game/Runtime/Domain/Tags/*`).

### 2) Modifier
Сущность:
- `ModifierId` (для удаления/обновления)
- `StatId Stat`
- `ModifierOp Op` (`AddFlat|AddPercent|Multiply|Override`)
- `float Value`
- `int Bucket` (для группировки additive/multiplicative или более тонко)
- `SourceRef Source` (ItemId, BuffId, AspectId, SkillId)

Функции:
- `bool IsValid()`

### 3) StatSheet (вычислитель)
Ответственность: поддерживать базовые значения и применённые моды, выдавать итог.

Ключевые функции:
- `SetBase(StatId, float baseValue)`
- `GetBase(StatId) -> float`
- `AddModifier(Modifier m) -> ModifierId`
- `RemoveModifier(ModifierId id)`
- `GetFinal(StatId) -> float` (с учётом buckets)
- `GetBreakdown(StatId) -> StatBreakdown` (список вкладов)
- `ClearTemporary(SourceRef scope)` (например, снять все моды от конкретного бафа)

### 4) SoftCaps
- `SoftCapRule { StatId, float Knee, float Max, CurveType }`
- Функция:
  - `ApplySoftCap(statId, rawValue) -> cappedValue`
- Требование:
  - deterministic, без аллокаций.

## Порядок применения (строго)
Рекомендуемая формула:
1) Start = Base
2) AddFlat: Sum(AddFlat)
3) AddPercent: Start *= (1 + Sum(AddPercent))
4) Multiply: Start *= Product(1 + Multiply) (или direct multiply)
5) Override: если есть — победитель по приоритету (например highest priority source)

## Временные моды
- `TimedModifier { Modifier, float RemainingSeconds }`
- Обновление через scheduler (не per-frame list scan для 1000+ модов).

## Тесты (EditMode)
- корректность порядка op’ов
- soft cap curve
- deterministic breakdown order
- removal by id
