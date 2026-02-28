# CURSOR_CHECKLIST.md (галочки, чтобы ничего не забыть)

Дата: 2026-02-27

Правило: **не переходить к следующему пункту, пока текущий не закрыт**.
Каждый пункт = отдельный коммит (или серия коммитов, если явно указано).

---

## 0) Preflight
- [ ] Прочитаны: `docs/INDEX.md`, `docs/CURSOR_MASTER_PROMPT.md`
- [ ] Прочитаны каноны: `specs/AAA_ARCHITECTURE_CANON.md`, `specs/UI_WINDOWING_FRAMEWORK.md`, `specs/UI_IMPLEMENTATION_PLAN_WINDOWING.md`
- [ ] Прочитаны PoE каноны: `specs/POE_CORE_REQUIREMENTS.md` + детальные спеки
- [ ] Прочитан D3 loot: `specs/LOOT_SYSTEM_D3.md`
- [ ] Созданы asmdef слоёв, зависимости корректны (Presentation→Application→Domain, Domain без Unity)
- [ ] Добавлен минимальный “fail fast” тест: Domain assembly не содержит Unity references

---

## 1) AAA Foundation (Этап 07)
### 1.1 Deterministic sim
- [ ] Fixed tick (SimTime + порядок систем)
- [ ] RNG streams (Combat/Loot/AI/World/Craft)
- [ ] Запрет UnityEngine.Random в Domain (линтер/ревью/тест)

### 1.2 Modifier Algebra v2
- [ ] Bucket model: Base/Add/Increased/More/Conversion/GainAsExtra/Cap
- [ ] Provenance: источник каждого модификатора (items/passives/gems/flasks/effects)
- [ ] DamageBreakdown: Base→Added→Increased→More→Conversion→GainAsExtra→Crit→Mitigation→Final

### 1.3 Perf
- [ ] PerfBudget (maxEnemies/maxProjectiles/maxLootLabels/maxUiRefreshPerFrame)
- [ ] Pools: projectiles/floating texts/loot actors/tooltips/toasts
- [ ] UIRefreshScheduler batching

### 1.4 Tests (EditMode)
- [ ] DeterminismRngTests
- [ ] ModifierAlgebraTests (increased/more/conversion order)
- [ ] DamageBreakdownTests

---

## 2) UI Windowing (Этап 08)
### 2.1 Core
- [ ] UIRoot single canvas + EventSystem + TMP
- [ ] WindowTemplate.prefab (Chrome/Content/Overlay)
- [ ] WindowManager + WindowRegistry + IWindowService

### 2.2 Behavior
- [ ] DragMove (anchoredPosition, canvas scale aware, clamp safe-area)
- [ ] Focus/z-order bring-to-front
- [ ] Modal blocker (click-through запрещён)
- [ ] Resize (min/max)
- [ ] Dock/Snap (edge snap минимум)

### 2.3 Persistence
- [ ] UILayoutState (pos/size/dock/collapsed/pinned/zIndex/activeTab/customJson)
- [ ] Save/restore layout (JSON)
- [ ] Layout autosave on end-drag/end-resize/close

### 2.4 Input & DnD
- [ ] InputContextStack (Gameplay/UI/ChatTyping/Modal)
- [ ] DragDropService (ItemRef/GemRef/SkillRef/CurrencyRef)
- [ ] VirtualizedListView + pooling

### 2.5 Tests (PlayMode)
- [ ] WindowingSmokeTests: открыть 20 окон, drag, save, reload

---

## 3) D3 Loot loop (в мир)
- [ ] LootGenerator (D3 профили)
- [ ] World loot actors + pooled labels
- [ ] ALT show toggle + highlight beams/colors (текстом/простыми объектами)
- [ ] Pickup transaction + inventory update
- [ ] Tests: loot smoke

---

## 4) PoE Core (Этап 09)
### 4.1 Itemization
- [ ] ItemBaseDefinition + implicit
- [ ] ModDefinition prefix/suffix + tiers + ilvl gating + group conflicts
- [ ] PoeItemGenerator deterministic (seed)
- [ ] ModPoolValidator (Editor+CI)

### 4.2 Sockets/Links + Gems
- [ ] SocketModel + LinkGroups
- [ ] SkillGemDefinition + SupportGemDefinition + GemInstance
- [ ] SupportEffectKind enum (каталог)
- [ ] SkillBuildCompiler:
  - [ ] supports только в linkGroup
  - [ ] rejected supports with reasons
  - [ ] stable order (Order then GemId)
  - [ ] Explain breakdown

### 4.3 Passive Tree
- [ ] PassiveTreeDefinition graph (nodes/edges/positions)
- [ ] Allocation rules (adjacency)
- [ ] Respec connectivity validator
- [ ] Search index (name/tag)

### 4.4 Flasks
- [ ] Flask charges spend/gain rules
- [ ] Effects via Modifier v2

### 4.5 Currency Actions
- [ ] CurrencyActionDefinition + engine
- [ ] Operations MVP: reroll/add/remove/bias/sockets/colors/links/quality/(corrupt optional)
- [ ] deterministic Craft RNG stream
- [ ] Ledger entry on apply

### 4.6 Tests
- [ ] SkillBuildCompilerTests
- [ ] PoeItemGeneratorTests
- [ ] PassiveTreeRulesTests
- [ ] CurrencyActionEngineTests

---

## 5) Application UseCases + Transactions
- [ ] TransactionRunner begin/validate/apply/publish/commit + idempotency
- [ ] InsertGemUseCase / RemoveGemUseCase
- [ ] Allocate/Respec Passive UseCases
- [ ] ApplyCurrencyActionUseCase (+ ledger)
- [ ] UseFlaskUseCase
- [ ] UI events published for all changes

---

## 6) UI окна PoE
- [ ] Inventory/Character окна работают в WindowTemplate
- [ ] Socket Inspector: drag&drop gems ↔ sockets + applied/rejected list
- [ ] Skills Window: compiled skills from gems + hotbar assign
- [ ] Passive Tree Window: zoom/pan/search/preview/allocate/respec
- [ ] Craft Window: apply actions + confirm irreversible
- [ ] Flask Belt HUD: 1..5 charges + hotkeys

PlayMode smoke:
- [ ] GemDragDropSmokeTests
- [ ] PassiveTreeAllocateSmokeTests
- [ ] CraftApplySmokeTests

---

## 7) Документация/прогресс
- [ ] На каждый коммит добавлена строка в `docs/PROGRESS.md`
- [ ] `docs/INDEX.md` и `docs/FILE_MAP.md` обновлены при добавлении файлов
- [ ] Новые спеки добавлены в `docs/INDEX.md`

---

## Definition of Done (финал)
- [ ] D3 loot loop в мире (labels/highlight/ALT) работает стабильно
- [ ] PoE ядро работает: gems/links/supports/skills/hotbar + passive tree + crafting + flasks
- [ ] UI windowing: drag/save/restore/focus/modal работает без лагов
- [ ] Domain без Unity, действия через транзакции, детерминизм по seed
- [ ] Валидация данных + тесты зелёные
