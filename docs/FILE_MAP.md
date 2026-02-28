# File Map (папки и ответственность)

## Assets/Game/Runtime
- Domain/ — чистая логика, правила, математика, модели, валидаторы.
  - Simulation/ — fixed-tick время и порядок симуляции.
  - Rng/ — детерминированные RNG и stream-провайдеры.
  - Modifiers/ — algebra v2 (bucket-агрегация, provenance, conversion ordering).
  - Combat/ — breakdown-модели (например, DamageBreakdown).
- Application/ — use cases, сервисы сценариев: equip/cast/generate/save.
- Presentation/ — Unity сцены, MonoBehaviours, UI Views, Input, NavMesh, VFX/Anim.
  - UI/Windowing/ — базовый windowing framework и планировщик UI refresh.
  - Common/ — инфраструктурные клиентские утилиты (например, object pooling).
- Infrastructure/ — IO, serialization, logging adapters, профилировщики.
  - Config/ — конфиги производительности (PerfBudget settings/provider).

## Assets/Game/Data
ScriptableObject definitions будут в `Assets/Game/Runtime/Presentation/Data` (cs),
а assets создаются в редакторе в:
- Settings/
- Items/
- Abilities/
- Statuses/
- Worlds/

## Assets/Game/Tests/EditMode
Юнит-тесты домена: stat math, determinism, budget validator, anti-exploit guards.

## docs/
Документация, прогресс, чек-листы, матрицы.

## Reports/
Автогенерируемые отчёты: DPS, budgets, баланс.
