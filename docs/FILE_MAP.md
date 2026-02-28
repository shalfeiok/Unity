# File Map (папки и ответственность)

## Assets/Game/Runtime
- Domain/ — чистая логика, правила, математика, модели, валидаторы.
  - Simulation/ — fixed-tick время и порядок симуляции.
  - Rng/ — детерминированные RNG и stream-провайдеры.
  - Modifiers/ — algebra v2 (bucket-агрегация, provenance, conversion ordering).
  - Combat/ — breakdown-модели (например, DamageBreakdown).
  - Loot/ — генерация drop-таблиц и детерминированный world loot pipeline.
  - Poe/ — PoE-ориентированные модели itemization/skills/passives/flasks/crafting.
    - Items/ — базы, пулы модов, генератор предметов, селектор и роллы.
    - Sockets+Gems/ — линковка сокетов, skill/support gems и компиляция билда.
    - Passives/ — дерево пассивов, аллокация/рефанд и правила связности.
    - Flasks/ — модель фласок и сервис зарядов/использования.
- Application/ — use cases, сервисы сценариев: equip/cast/generate/save.
  - Transactions/ — transaction runner, idempotency и orchestration use case pipeline.
  - Poe/UseCases/ — PoE сценарии: gem/passive/flask/currency operations.
- Presentation/ — Unity сцены, MonoBehaviours, UI Views, Input, NavMesh, VFX/Anim.
  - UI/Windowing/ — базовый windowing framework и планировщик UI refresh.
    - Interactions/ — drag/focus/modal/resize/dock и layout persistence helpers.
    - Input/ — input context stack, drag&drop payload service, virtualized list view.
  - Common/ — инфраструктурные клиентские утилиты (например, object pooling).
  - World/ — world-actors для лута и других in-world presentation сущностей.
- Infrastructure/ — IO, serialization, logging adapters, профилировщики.
  - Config/ — конфиги производительности (PerfBudget settings/provider).
  - Persistence/ — JSON storage и репозитории сохранения layout/state.
  - DataPipeline/ — валидация таблиц данных и контентных пулов.
  - Economy/ — append-only ledger и audit trail экономики.

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
