# File Map (папки и ответственность)

## Assets/Game/Runtime
- Domain/ — чистая логика, правила, математика, модели, валидаторы.
  - Simulation/ — fixed-tick время и порядок симуляции.
  - Rng/ — детерминированные RNG и stream-провайдеры.
  - Modifiers/ — algebra v2 (bucket-агрегация, provenance, conversion ordering).
  - Combat/ — breakdown-модели (например, DamageBreakdown).
  - Loot/ — генерация drop-таблиц и детерминированный world loot pipeline.
  - Poe/ — PoE-ориентированные модели itemization/skills/passives/flasks/crafting.
    - Items/ — базы, implicits, пулы модов, генератор предметов, сокеты/связки/качество и селектор роллов.
    - Sockets+Gems/ — линковка сокетов, skill/support gems и компиляция билда.
    - SocketService/ — операции вставки/удаления gem в сокетах.
    - Passives/ — дерево пассивов, аллокация/рефанд, правила связности и поисковый индекс по id/name/tags.
    - Flasks/ — модель фласок, зарядов/использования и эффектов через Modifier v2.
- Application/ — use cases, сервисы сценариев: equip/cast/generate/save.
  - Transactions/ — transaction runner, idempotency и orchestration use case pipeline.
  - Events/ — шина application-событий (publish после успешных use case операций).
  - Poe/UseCases/ — PoE сценарии: gem/passive/flask/currency operations (вкл. InsertGem/RemoveGem/RefundPassive).
  - Loot/UseCases/ — прикладные сценарии подбора лута (transaction + inventory update + events).
  - UI/UseCases/ — прикладные UI операции (например, hotbar assignment).
- Presentation/ — Unity сцены, MonoBehaviours, UI Views, Input, NavMesh, VFX/Anim.
  - UI/Windowing/ — базовый windowing framework и планировщик UI refresh.
  - UI/Localization/ — словари и сервис локализации интерфейса (RU по умолчанию).
  - UI/Tooltips/ — модели и билдеры подсказок для предметов/статов/действий (включая explain по support-гемам).
  - UI/Windows/ — прикладные окна (Inventory/Character/SocketInspector и др.).
    - Skills/ — compiled skills panel и данные для биндинга на hotbar.
    - PassiveTree/ — состояние и сервисы окна дерева пассивов (zoom/pan/search/preview).
    - Craft/ — состояние и сервисы окна применения currency actions.
  - UI/Hud/ — HUD сервисы (hotbar bindings и боевые индикаторы).
    - Flasks/ — flask belt HUD состояние и обновление charges/usability.
    - Interactions/ — drag/focus/modal/resize/dock и layout persistence helpers.
    - Layout autosave hooks/ — автосохранение layout на завершении drag/resize/close.
    - Input/ — input context stack, drag&drop payload service, virtualized list view.
  - Common/ — инфраструктурные клиентские утилиты (например, object pooling).
  - World/ — world-actors для лута и других in-world presentation сущностей.
- Infrastructure/ — IO, serialization, logging adapters, профилировщики.
  - Config/ — конфиги производительности (PerfBudget settings/provider) и enforcement лимитов.
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

## Assets/Game/Tests/PlayMode
Smoke/интеграционные тесты UI и пользовательских сценариев (dragdrop/passive/craft и т.п.).

## docs/
Документация, прогресс, чек-листы, матрицы.

## Reports/
Автогенерируемые отчёты: DPS, budgets, баланс.
