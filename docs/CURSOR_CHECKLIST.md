# CURSOR_CHECKLIST.md (галочки, чтобы ничего не забыть)

Дата: 2026-02-27

Правило: **не переходить к следующему пункту, пока текущий не закрыт**.
Каждый пункт = отдельный коммит (или серия коммитов, если явно указано).

---

## 0) Preflight
- [x] Прочитаны: `docs/INDEX.md`, `docs/CURSOR_MASTER_PROMPT.md`
- [x] Прочитаны каноны: `specs/AAA_ARCHITECTURE_CANON.md`, `specs/UI_WINDOWING_FRAMEWORK.md`, `specs/UI_IMPLEMENTATION_PLAN_WINDOWING.md`
- [x] Прочитаны PoE каноны: `specs/POE_CORE_REQUIREMENTS.md` + детальные спеки
- [x] Прочитан D3 loot: `specs/LOOT_SYSTEM_D3.md`
- [x] Созданы asmdef слоёв, зависимости корректны (Presentation→Application→Domain, Domain без Unity)
- [x] Добавлен минимальный “fail fast” тест: Domain assembly не содержит Unity references

---

## 1) AAA Foundation (Этап 07)
### 1.1 Deterministic sim
- [x] Fixed tick (SimTime + порядок систем)
- [x] RNG streams (Combat/Loot/AI/World/Craft)
- [x] Запрет UnityEngine.Random в Domain (линтер/ревью/тест)

### 1.2 Modifier Algebra v2
- [x] Bucket model: Base/Add/Increased/More/Conversion/GainAsExtra/Cap
- [x] Provenance: источник каждого модификатора (items/passives/gems/flasks/effects)
- [x] DamageBreakdown: Base→Added→Increased→More→Conversion→GainAsExtra→Crit→Mitigation→Final

### 1.3 Perf
- [x] PerfBudget (maxEnemies/maxProjectiles/maxLootLabels/maxUiRefreshPerFrame)
- [ ] Pools: projectiles/floating texts/loot actors/tooltips/toasts
- [x] UIRefreshScheduler batching

### 1.4 Tests (EditMode)
- [x] DeterminismRngTests
- [x] ModifierAlgebraTests (increased/more/conversion order)
- [x] DamageBreakdownTests

---

## 2) UI Windowing (Этап 08)
### 2.1 Core
- [ ] UIRoot single canvas + EventSystem + TMP
- [ ] WindowTemplate.prefab (Chrome/Content/Overlay)
- [x] WindowManager + WindowRegistry + IWindowService

### 2.2 Behavior
- [x] DragMove (anchoredPosition, canvas scale aware, clamp safe-area)
- [x] Focus/z-order bring-to-front
- [ ] Modal blocker (click-through запрещён)
- [x] Resize (min/max)
- [x] Dock/Snap (edge snap минимум)

### 2.3 Persistence
- [x] UILayoutState (pos/size/dock/collapsed/pinned/zIndex/activeTab/customJson)
- [x] Save/restore layout (JSON)
- [ ] Layout autosave on end-drag/end-resize/close

### 2.4 Input & DnD
- [x] InputContextStack (Gameplay/UI/ChatTyping/Modal)
- [x] DragDropService (ItemRef/GemRef/SkillRef/CurrencyRef)
- [x] VirtualizedListView + pooling

### 2.5 Tests (PlayMode)
- [x] WindowingSmokeTests: открыть 20 окон, drag, save, reload

---

## 3) D3 Loot loop (в мир)
- [x] LootGenerator (D3 профили)
- [x] World loot actors + pooled labels
- [x] ALT show toggle + highlight beams/colors (текстом/простыми объектами)
- [x] Pickup transaction + inventory update
- [ ] Tests: loot smoke

---

## 4) PoE Core (Этап 09)
### 4.1 Itemization
- [x] ItemBaseDefinition + implicit
- [x] ModDefinition prefix/suffix + tiers + ilvl gating + group conflicts
- [x] PoeItemGenerator deterministic (seed)
- [x] ModPoolValidator (Editor+CI)

### 4.2 Sockets/Links + Gems
- [x] SocketModel + LinkGroups
- [x] SkillGemDefinition + SupportGemDefinition + GemInstance
- [x] SupportEffectKind enum (каталог)
- [x] SkillBuildCompiler:
  - [x] supports только в linkGroup
  - [x] rejected supports with reasons
  - [x] stable order (Order then GemId)
  - [x] Explain breakdown

### 4.3 Passive Tree
- [x] PassiveTreeDefinition graph (nodes/edges/positions)
- [x] Allocation rules (adjacency)
- [x] Respec connectivity validator
- [x] Search index (name/tag)

### 4.4 Flasks
- [x] Flask charges spend/gain rules
- [x] Effects via Modifier v2

### 4.5 Currency Actions
- [x] CurrencyActionDefinition + engine
- [x] Operations MVP: reroll/add/remove/bias/sockets/colors/links/quality/(corrupt optional)
- [x] deterministic Craft RNG stream
- [x] Ledger entry on apply

### 4.6 Tests
- [x] SkillBuildCompilerTests
- [x] PoeItemGeneratorTests
- [x] PassiveTreeRulesTests
- [x] CurrencyActionEngineTests

---

## 5) Application UseCases + Transactions
- [x] TransactionRunner begin/validate/apply/publish/commit + idempotency
- [x] InsertGemUseCase / RemoveGemUseCase
- [x] Allocate/Respec Passive UseCases
- [x] ApplyCurrencyActionUseCase (+ ledger)
- [x] UseFlaskUseCase
- [x] UI events published for all changes

---

## 6) UI окна PoE
- [x] Inventory/Character окна работают в WindowTemplate
- [x] Socket Inspector: drag&drop gems ↔ sockets + applied/rejected list
- [x] Skills Window: compiled skills from gems + hotbar assign
- [x] Passive Tree Window: zoom/pan/search/preview/allocate/respec
- [x] Craft Window: apply actions + confirm irreversible
- [x] Flask Belt HUD: 1..5 charges + hotkeys

PlayMode smoke:
- [x] GemDragDropSmokeTests
- [x] PassiveTreeAllocateSmokeTests
- [x] CraftApplySmokeTests

---

## 7) Документация/прогресс
- [x] На каждый коммит добавлена строка в `docs/PROGRESS.md`
- [x] `docs/INDEX.md` и `docs/FILE_MAP.md` обновлены при добавлении файлов
- [x] Новые спеки добавлены в `docs/INDEX.md`

---

## Definition of Done (финал)
- [ ] D3 loot loop в мире (labels/highlight/ALT) работает стабильно
- [ ] PoE ядро работает: gems/links/supports/skills/hotbar + passive tree + crafting + flasks
- [ ] UI windowing: drag/save/restore/focus/modal работает без лагов
- [ ] Domain без Unity, действия через транзакции, детерминизм по seed
- [ ] Валидация данных + тесты зелёные
