# Statuses: DoT/HoT/Slow/Stun/Freeze/Poison + tick scheduler


## Цели
- Единый статусный движок, расширяемый через definitions.
- Правила стаков: refresh duration, add stacks, independent instances.
- Оптимизированный тик: scheduler с bucket intervals.
- События apply/expire/tick.

## StatusDefinition (data)
Поля:
- Id, Name
- Tags (например `Status.Burn`)
- DurationSeconds
- TickIntervalSeconds (0 = no tick)
- StackPolicy:
  - MaxStacks
  - OnApply: Refresh|AddStack|Replace|Ignore
  - OnMaxStacks: Clamp|ReplaceOldest|ConvertToX
- Effects:
  - Stat modifiers while active (например slow)
  - Tick effect (DamagePacket/Heal)
  - OnExpire effect (optional)

## Runtime model
- `StatusInstance { StatusId, Source, Target, Remaining, Stacks, NextTickAt }`

## Tick scheduler
Интерфейс:
- `Schedule(StatusInstance)`
- `Advance(timeNow)` → returns list of due ticks (or invokes callbacks)

Требование:
- без аллокаций: используем массивы/пулы/кольцевые буферы.

## Apply/Remove API
- `TryApply(StatusApplyRequest) -> StatusApplyResult`
- `RemoveById(StatusInstanceId)`
- `RemoveByTag(TagId)` (opц)
- `GetActiveStatuses(EntityId) -> readonly span/list`

## Stack policies (должны быть документированы)
- Burn: refresh duration + clamp stacks
- Poison: independent instances (опц)
- Freeze: replace stronger, suppress weaker

## Тесты
- stack logic
- tick schedule correctness
- expire triggers
- deterministic ordering
