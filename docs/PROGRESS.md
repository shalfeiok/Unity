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
- 2026-02-28: Добавлены window interactions: WindowDragMove (canvas-scale aware), WindowFocusController, WindowModalBlocker, WindowResize, WindowDockSnap + EditMode тесты на drag/resize/snap/focus/modal.
- 2026-02-28: Добавлены UILayoutState/UILayoutPersistence и Infrastructure persistence слой (JsonStorage + UILayoutRepository) с тестом save/load roundtrip.
- 2026-02-28: Добавлены InputContextStack (Gameplay/UI/ChatTyping/Modal), DragDropService (ItemRef/GemRef/SkillRef/CurrencyRef), VirtualizedListView<T> и EditMode тесты.
- 2026-02-28: Реализован старт D3-like loot loop: Domain LootGenerator/LootDrop/LootRarity (детерминированный drop), Presentation LootWorldActor/LootLabel и LootAltToggle + EditMode тесты по детерминизму и ALT visibility.
- 2026-02-28: Начат блок PoE itemization: ItemBaseDefinition/ModDefinition, ModPoolSelector, PoeItemGenerator и ModPoolValidator + EditMode тесты (caps 3/3, deterministic seed, ilvl gating, duplicate mod ids).
- 2026-02-28: Реализован блок PoE gems/links: SocketModel/LinkGroup, SkillGemDefinition/SupportGemDefinition/GemInstance, SkillBuildCompiler (applied/rejected + deterministic ordering) и EditMode тесты.
- 2026-02-28: Реализован блок Passive Tree: PassiveTreeDefinition, PassiveTreeService (allocate/refund with connectivity checks), PassiveTreeValidator и EditMode тесты.
- 2026-02-28: Реализован блок Flasks: FlaskDefinition, FlaskService (charges/use/gain clamp) и EditMode тесты.
- 2026-02-28: Реализован блок PoE currency actions: CurrencyActionDefinition/CurrencyActionEngine (reroll/add/remove с детерминированным craft stream) и append-only TransactionLedger + EditMode тесты.
- 2026-02-28: Реализован Application слой транзакций: TransactionRunner (idempotency) и PoE use cases (ApplyCurrencyAction, AllocatePassiveNode, UseFlask) + EditMode тесты.
- 2026-02-28: Добавлены gem use cases уровня Application (InsertGem/RemoveGem) и Domain SocketService, плюс EditMode тесты для вставки/извлечения и idempotency через TransactionRunner.
- 2026-02-28: Добавлен базовый UI слой окон Inventory/Character и SocketInspectorService (dragdrop gems, remove to inventory, compile applied/rejected supports) + EditMode тесты.
- 2026-02-28: Добавлен блок Skills Panel + Hotbar binding: SkillsPanelState/Entry/Service (отображение compiled skills и rejected reasons), HotbarAssignmentService (assign/unassign/snapshot) и EditMode тесты.
- 2026-02-28: Добавлен блок Passive Tree Window: состояние окна (zoom/pan/search/highlight/preview), сервис интеракций и EditMode тесты на clamp/панораму/поиск/preview.
- 2026-02-28: Добавлены Craft Window и Flask Belt HUD базовые сервисы/состояния: предпросмотр и применение currency action в окне крафта, отображение charges/usability для flask slots, плюс EditMode тесты.
- 2026-02-28: Добавлен блок PlayMode smoke tests для PoE UI-флоу: GemDragDropSmokeTests, PassiveTreeAllocateSmokeTests, CraftApplySmokeTests.
- 2026-02-28: Добавлены PlayMode WindowingSmokeTests: smoke на open/toggle окон и save/load layout с проверкой drag-позиции.
- 2026-02-28: Добавлен RefundPassiveNodeUseCase (respec) в Application слой с EditMode тестом на соблюдение правил связности дерева при возврате нод.
- 2026-02-28: Добавлен application use case AssignSkillToHotbarUseCase (assign/unassign с TransactionRunner idempotency) и EditMode тесты.
