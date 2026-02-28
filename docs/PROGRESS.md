# PROGRESS (журнал работ)

> Заполняется по мере выполнения задач. Формат обязателен.

## Шаблон
### TASK <ID> — <Название>
- Статус: TODO | IN PROGRESS | DONE | BLOCKED
- Что сделано:
  - ...
- Файлы:
  - ...
- Тесты:
  - ...
- Ручная проверка:
  1) ...
- Риски/заметки:
  - ...

---
- 2026-02-27: Добавлены расширенные спеки под Lineage II + Diablo III (UI окна/интеракции/loot/матрица покрытия) и обновлён этап 03_UI.
- 2026-02-27: Добавлены AAA канон-спеки (network/determinism/persistence/ledger/data pipeline/breakdown/perf/tooling/observability/release), UI windowing framework и ядро PoE; добавлены этапы 07-10.
- 2026-02-27: Добавлена атомарная детализация PoE ядра (gems/links, passive tree, item mods, currency actions) и атомарный план реализации windowing UI.
- 2026-02-27: Добавлены docs/ARCHIVE_ACTIONS_MAP.md и docs/CURSOR_EXECUTION_SCRIPT.md (полная карта действий и пошаговый скрипт для Cursor).
- 2026-02-27: Добавлены docs/CURSOR_CHECKLIST.md, docs/DEFINITION_OF_READY_DONE.md, docs/STAGE_ACCEPTANCE_MATRIX.md для жёсткого контроля исполнения.
- 2026-02-28: Выполнен первый проход preflight/AAA foundation: добавлены layered asmdef, fixed-tick SimulationLoop/SimTime, детерминированные RNG streams (RngProvider + RngStreamId) и EditMode тесты для симуляции и детерминизма.
- 2026-02-28: Реализована Modifier Algebra v2: новые buckets (Add/Increased/More/Conversion/Cap), provenance-упорядочивание conversion по sourceId, интеграция в StatSheet и DamageBreakdown + EditMode тесты (sum increased / multiply more / stable conversion order).
- 2026-02-28: Добавлены базовые perf-инструменты: UIRefreshScheduler (budgeted batching), generic ObjectPool<T>, PerfBudgetSettings/Provider для загрузки бюджетов из Resources + EditMode тесты scheduler/pool.
- 2026-02-28: Старт windowing framework: добавлены WindowId, WindowRegistry, IWindowService/WindowService, WindowManager, WindowRoot (MonoBehaviour) и EditMode тест на open/close state.
