# CURSOR_EXECUTION_SCRIPT.md (выполнение без “думать”)

Дата: 2026-02-27

Ниже — **пошаговый скрипт**, который Cursor обязан выполнить в корне Unity проекта.  
Правило: **1 задача = 1 коммит**. Задач много — коммитов много.

---

## 0) Preflight (Commit 000: setup)
### 0.1 Прочитать канон
Прочитать в таком порядке:
1) `docs/INDEX.md`
2) `docs/CURSOR_MASTER_PROMPT.md`
3) `specs/AAA_ARCHITECTURE_CANON.md`
4) `specs/UI_WINDOWING_FRAMEWORK.md`
5) `specs/UI_IMPLEMENTATION_PLAN_WINDOWING.md`
6) `specs/POE_CORE_REQUIREMENTS.md`
7) `specs/POE_GEMS_LINKS_DETAILED.md`
8) `specs/POE_PASSIVE_TREE_DETAILED.md`
9) `specs/POE_ITEM_MODS_PREFIX_SUFFIX_DETAILED.md`
10) `specs/POE_CURRENCY_ACTIONS_CATALOG.md`
11) `specs/LOOT_SYSTEM_D3.md`

### 0.2 Создать слои и asmdef
Создать/проверить assembly definitions:
- `Game.Domain.asmdef`
- `Game.Application.asmdef` (depends on Game.Domain)
- `Game.Infrastructure.asmdef` (depends on Game.Domain + Game.Application)
- `Game.Presentation.asmdef` (depends on Game.Application)

Проверка:
- Domain не имеет reference на Unity assemblies.

Commit message: `chore: add layered asmdefs and enforce dependencies`

---

## 1) AAA Foundation (серия коммитов)
### 1.1 Fixed tick + SimTime
Файлы:
- `Domain/Simulation/SimTime.cs` (tickIndex, dt)
- `Domain/Simulation/SimulationLoop.cs` (order of systems)

Действия:
- Ввести fixed step update, используемый Domain симуляцией.

Commit: `feat(domain): add fixed-tick simulation time`

### 1.2 RNG streams (детерминизм)
Файлы:
- `Domain/Rng/IRng.cs`
- `Domain/Rng/RngStreamId.cs` (Combat/Loot/AI/World/Craft)
- `Domain/Rng/RngProvider.cs` (seed + substreams)

Тесты:
- EditMode: 100 повторов → одинаковые последовательности.

Commit: `feat(domain): deterministic rng streams`

### 1.3 Modifier Algebra v2 + Breakdown
Файлы:
- `Domain/Modifiers/Modifier.cs` (sourceId, scopeTags, bucket, value)
- `Domain/Modifiers/ModifierBucket.cs` (Base/Add/Increased/More/Conversion/GainAsExtra/Cap)
- `Domain/Modifiers/ModifierSet.cs` (aggregate + provenance)
- `Domain/Stats/StatSheet.cs` (compute)
- `Domain/Combat/DamageBreakdown.cs`

Тесты:
- increased суммируется
- more перемножается
- conversion order стабильный

Commit: `feat(domain): modifier algebra v2 with provenance and breakdown`

### 1.4 PerfBudget + Pools + UI batching
Файлы (Presentation/Infrastructure):
- `Presentation/UI/Windowing/UIRefreshScheduler.cs`
- `Presentation/Common/ObjectPool.cs`
- `Infrastructure/Config/PerfBudget.asset` (+ loader)

Commit: `feat: add perf budgets, pooling, and ui batching`

---

## 2) Windowing UI Framework (серия коммитов)
### 2.1 UIRoot + WindowManager
Файлы:
- `Presentation/UI/Windowing/WindowId.cs`
- `Presentation/UI/Windowing/WindowRegistry.cs`
- `Presentation/UI/Windowing/IWindowService.cs`
- `Presentation/UI/Windowing/WindowService.cs`
- `Presentation/UI/Windowing/WindowManager.cs`
- `Presentation/UI/Windowing/WindowRoot.cs`
Префаб:
- `Presentation/UI/Windowing/Prefabs/WindowTemplate.prefab`

Commit: `feat(ui): add window manager and window template`

### 2.2 DragMove/Focus/Modal/Resize/Dock
Файлы:
- `WindowDragMove.cs` (anchoredPosition, canvas-scale aware)
- `WindowFocusController.cs`
- `WindowModalBlocker.cs`
- `WindowResize.cs`
- `WindowDockSnap.cs`

Commit: `feat(ui): add stable draggable windows with focus/modal/resize/snap`

### 2.3 UILayout persistence
Файлы:
- `UILayoutState.cs` (WindowState with pos/size/dock/collapsed/pinned/zIndex/activeTab/customJson)
- `UILayoutPersistence.cs` (JSON save/load)
Infrastructure:
- `Infrastructure/Persistence/JsonStorage.cs`
- `Infrastructure/Persistence/UILayoutRepository.cs`

Commit: `feat(ui): persist and restore window layouts`

### 2.4 InputContextStack + DragDropService + VirtualizedList
Файлы:
- `InputContextStack.cs` (Gameplay/UI/ChatTyping/Modal)
- `DragDropService.cs` (payload: ItemRef/GemRef/SkillRef/CurrencyRef)
- `VirtualizedListView.cs`

Commit: `feat(ui): add input context stack, dragdrop service, virtualized lists`

### 2.5 PlayMode smoke test
Файлы:
- `Tests/PlayMode/UI/WindowingSmokeTests.cs` (open 20 windows, drag, save, reload)

Commit: `test(ui): add windowing smoke tests`

---

## 3) D3 Loot loop “в мир” (серия коммитов)
### 3.1 Loot generation + world labels + ALT show
Использовать `specs/LOOT_SYSTEM_D3.md` и `specs/UI_INTERACTIONS_L2_D3.md`.

Файлы:
- `Domain/Loot/LootGenerator.cs`
- `Presentation/World/LootWorldActor.cs`
- `Presentation/UI/Widgets/LootLabel.cs` (pool)
- `Presentation/Input/LootAltToggle.cs`

Commit: `feat(loot): implement d3-like loot drop with world labels and highlight`

---

## 4) PoE Core (серия коммитов, строго порядок)
### 4.1 Poe itemization: bases/mod pools/tiers/ilvl
Файлы:
- `Domain/Poe/Items/ItemBaseDefinition.cs`
- `Domain/Poe/Items/ModDefinition.cs`
- `Domain/Poe/Items/PoeItemGenerator.cs`
- `Domain/Poe/Items/ModPoolSelector.cs`
- `Infrastructure/DataPipeline/ModPoolValidator.cs`

Тесты:
- caps 3/3
- group conflicts
- ilvl gating
- deterministic seed

Commit: `feat(poe): add poe-like item bases and prefix/suffix tiers`

### 4.2 Sockets/Links + Gems + Compiler
Файлы:
- `Domain/Poe/Sockets/SocketModel.cs`
- `Domain/Poe/Sockets/LinkGroup.cs`
- `Domain/Poe/Gems/SkillGemDefinition.cs`
- `Domain/Poe/Gems/SupportGemDefinition.cs`
- `Domain/Poe/Gems/GemInstance.cs`
- `Domain/Poe/Gems/SupportEffectKind.cs`
- `Domain/Poe/Gems/SkillBuildCompiler.cs` (Explain: applied/rejected)

Тесты:
- supports only in linkGroup
- rejected reasons exist
- stable ordering

Commit: `feat(poe): implement gems, links and deterministic skill compiler`

### 4.3 Passive Tree
Файлы:
- `Domain/Poe/Passives/PassiveTreeDefinition.cs`
- `Domain/Poe/Passives/PassiveTreeService.cs`
- `Infrastructure/DataPipeline/PassiveTreeValidator.cs`

Commit: `feat(poe): add passive tree model, rules, and validators`

### 4.4 Flasks
Файлы:
- `Domain/Poe/Flasks/FlaskDefinition.cs`
- `Domain/Poe/Flasks/FlaskService.cs`

Commit: `feat(poe): add flask charges and effects`

### 4.5 Currency actions (crafting engine + ledger)
Файлы:
- `Domain/Poe/Crafting/CurrencyActionDefinition.cs`
- `Domain/Poe/Crafting/CurrencyActionEngine.cs` (deterministic craft stream)
- `Infrastructure/Economy/TransactionLedger.cs` (append-only)

Commit: `feat(poe): implement currency action crafting with ledger`

---

## 5) Application UseCases (серия коммитов)
Файлы:
- `Application/Transactions/TransactionRunner.cs` (begin/validate/apply/publish/commit + idempotency)
- `Application/Poe/UseCases/InsertGemUseCase.cs`
- `Application/Poe/UseCases/RemoveGemUseCase.cs`
- `Application/Poe/UseCases/AllocatePassiveNodeUseCase.cs`
- `Application/Poe/UseCases/ApplyCurrencyActionUseCase.cs`
- `Application/Poe/UseCases/UseFlaskUseCase.cs`
- (и другие из каталога L2/D3)

Commit: `feat(app): add poe usecases and transactional runner`

---

## 6) UI окна PoE (серия коммитов)
### 6.1 Inventory/Character + Socket Inspector
Файлы:
- `Presentation/UI/Windows/Inventory/*`
- `Presentation/UI/Windows/Character/*`
- `Presentation/UI/Windows/Inventory/SocketInspector/*`

Требования:
- drag&drop gems ↔ sockets через DragDropService
- отображать applied/rejected supports reasons

Commit: `feat(ui): add socket inspector and gem dragdrop`

### 6.2 Skills Panel + Hotbar binding
Commit: `feat(ui): list compiled skills from gems and support hotbar assignment`

### 6.3 Passive Tree Window (zoom/pan/search/preview)
Commit: `feat(ui): implement poe-style passive tree window`

### 6.4 Craft Window
Commit: `feat(ui): implement poe currency crafting window`

### 6.5 Flask Belt HUD
Commit: `feat(ui): add flask belt hud with charges`

---

## 7) Tests (финальный блок)
EditMode:
- SkillBuildCompilerTests
- PoeItemGeneratorTests
- PassiveTreeRulesTests
- CurrencyActionEngineTests
- ModifierAlgebraTests

PlayMode:
- GemDragDropSmokeTests
- PassiveTreeAllocateSmokeTests
- CraftApplySmokeTests

Commit: `test: add poe core domain and ui smoke tests`

---

## 8) Документация и прогресс
На каждый коммит:
- обновлять `docs/PROGRESS.md` строкой “что сделано”.
- если добавлены новые файлы/папки — обновить `docs/INDEX.md` и `docs/FILE_MAP.md`.

Commit: `docs: update progress and index`

---

## Done definition
Функционально:
- D3 loot loop в мире (labels/highlight/ALT) работает.
- PoE gems/links/supports/skills/hotbar работает.
- Passive tree allocation/respec работает.
- Currency crafting и flasks работают.
- UI windowing: drag/save/restore/focus/modal работает.

Архитектурно:
- Domain без Unity
- транзакции для действий
- детерминизм по seed
- валидаторы данных + тесты
