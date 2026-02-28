# ARCHIVE_ACTIONS_MAP.md

Дата: 2026-02-27

Этот файл — **единственный “источник правды” для Cursor**: что читать, что создавать, что менять, и **какие конкретные действия** выполнять, чтобы реализовать гибрид **Lineage 2 + Diablo 3 + Path of Exile (ядро)** в Unity 6000 LTS.

> Важно: архив — это **спеки/план/канон**. Реальная реализация делается **в корне вашего Unity-проекта**, соблюдая слои и этапы.

---

## 1) ЧТО ДЕЛАТЬ С КАЖДЫМ ФАЙЛОМ АРХИВА

### 1.1 Root
- `README.md`

Действия:
- `README.md` — entrypoint: держать краткий смысл проекта, ссылки на канон, как запускать прототип.

---

### 1.2 docs/ (правила, навигация, workflow)
- `docs/ARCHITECTURE.md`
- `docs/COMPATIBILITY.md`
- `docs/CURSOR_MASTER_PROMPT.md`
- `docs/FILE_MAP.md`
- `docs/GLOSSARY.md`
- `docs/INDEX.md`
- `docs/PERF_BUDGET.md`
- `docs/PRODUCTION_SCOPE.md`
- `docs/PROGRESS.md`
- `docs/SMOKE_TESTS.md`
- `docs/SOURCE_ORIGINAL_PROMPT.md`
- `docs/WORKFLOW.md`

Действия (строго):
- `docs/INDEX.md` — навигация. Любой новый важный файл обязан быть добавлен сюда.
- `docs/CURSOR_MASTER_PROMPT.md` — правила работы Cursor. Любая новая “каноническая” часть обязана быть добавлена в блок “читать перед реализацией”.
- `docs/WORKFLOW.md` — 1 задача = 1 коммит, прогресс фиксировать в `docs/PROGRESS.md`.
- `docs/FILE_MAP.md` — логическая карта проекта (если добавляешь новые папки/модули — обновить).
- `docs/SMOKE_TESTS.md` — обязателен минимальный набор PlayMode smoke тестов.
- `docs/PERF_BUDGET.md` — лимиты. Любая новая подсистема должна уважать лимиты (labels, UI refresh, pools).
- `docs/GLOSSARY.md` — термины (L2/D3/PoE/AAA). Любая новая сущность → добавить термин.

---

### 1.3 specs/ (канон требований)
- `specs/AAA_ARCHITECTURE_CANON.md`
- `specs/Abilities_Graph.md`
- `specs/COMBAT_BREAKDOWN_AND_DEATH_RECAP.md`
- `specs/Combat.md`
- `specs/DATA_PIPELINE_VALIDATION_HOTFIX.md`
- `specs/DETERMINISTIC_SIMULATION_AND_REPLAYS.md`
- `specs/ECONOMY_LEDGER_ANTI_DUPE.md`
- `specs/Enemies_AI.md`
- `specs/FEATURE_MATRIX_L2_D3.md`
- `specs/InputCamera.md`
- `specs/Items.md`
- `specs/LOOT_SYSTEM_D3.md`
- `specs/NETWORK_REPLICATION_AAA.md`
- `specs/OBSERVABILITY_LOGS_METRICS_TRACES.md`
- `specs/PERFORMANCE_BUDGETING_AND_POOLS.md`
- `specs/PERSISTENCE_SCHEMA_MIGRATIONS.md`
- `specs/POE_CORE_REQUIREMENTS.md`
- `specs/POE_CURRENCY_ACTIONS_CATALOG.md`
- `specs/POE_GEMS_LINKS_DETAILED.md`
- `specs/POE_ITEM_MODS_PREFIX_SUFFIX_DETAILED.md`
- `specs/POE_PASSIVE_TREE_DETAILED.md`
- `specs/Performance_QA.md`
- `specs/RELEASE_CHANNELS_COMPAT_ROLLBACK.md`
- `specs/SaveLoad.md`
- `specs/Skillcraft.md`
- `specs/Stats.md`
- `specs/Statuses.md`
- `specs/TOOLING_GM_SIMULATORS.md`
- `specs/Tooling_Reports.md`
- `specs/UI.md`
- `specs/UI_IMPLEMENTATION_PLAN_WINDOWING.md`
- `specs/UI_INTERACTIONS_L2_D3.md`
- `specs/UI_WINDOWING_FRAMEWORK.md`
- `specs/UI_WINDOWS_CATALOG_L2_D3.md`
- `specs/VFX_Animation.md`
- `specs/Worlds_Biomes_POI.md`

Действия:
- Никаких “вольных трактовок”: реализация должна соответствовать контрактам в спеках.
- Если есть выбор реализации — выбирать **лучший вариант** из `specs/AAA_ARCHITECTURE_CANON.md` и `specs/UI_WINDOWING_FRAMEWORK.md`.
- Любые новые поля/DTO добавлять сначала в спек, затем в код.

Ключевые “каноны”:
- `specs/AAA_ARCHITECTURE_CANON.md` — границы слоёв, транзакции, детерминизм, data pipeline.
- `specs/UI_WINDOWING_FRAMEWORK.md` + `specs/UI_IMPLEMENTATION_PLAN_WINDOWING.md` — стабильные перемещаемые окна.
- `specs/POE_CORE_REQUIREMENTS.md` + детальные PoE спеки — ядро PoE.

---

### 1.4 stages/ (логика разработки по этапам)
- `stages/00_Vision/ACCEPTANCE.md`
- `stages/00_Vision/OVERVIEW.md`
- `stages/00_Vision/TASKS.md`
- `stages/01_Foundation/ACCEPTANCE.md`
- `stages/01_Foundation/OVERVIEW.md`
- `stages/01_Foundation/TASKS.md`
- `stages/02_Movement_Camera_Input/ACCEPTANCE.md`
- `stages/02_Movement_Camera_Input/OVERVIEW.md`
- `stages/02_Movement_Camera_Input/TASKS.md`
- `stages/03_UI/ACCEPTANCE.md`
- `stages/03_UI/OVERVIEW.md`
- `stages/03_UI/README.md`
- `stages/03_UI/TASKS.md`
- `stages/04_Domain_Core/ACCEPTANCE.md`
- `stages/04_Domain_Core/OVERVIEW.md`
- `stages/04_Domain_Core/TASKS.md`
- `stages/05_EndToEnd_Combat_Loot_Craft/ACCEPTANCE.md`
- `stages/05_EndToEnd_Combat_Loot_Craft/OVERVIEW.md`
- `stages/05_EndToEnd_Combat_Loot_Craft/TASKS.md`
- `stages/06_Worlds_Portals_SaveLoad/ACCEPTANCE.md`
- `stages/06_Worlds_Portals_SaveLoad/OVERVIEW.md`
- `stages/06_Worlds_Portals_SaveLoad/TASKS.md`
- `stages/07_AAA_FOUNDATION/README.md`
- `stages/07_Tooling_Perf_QA_CI/ACCEPTANCE.md`
- `stages/07_Tooling_Perf_QA_CI/OVERVIEW.md`
- `stages/07_Tooling_Perf_QA_CI/TASKS.md`
- `stages/08_Content_Pack/ACCEPTANCE.md`
- `stages/08_Content_Pack/OVERVIEW.md`
- `stages/08_Content_Pack/TASKS.md`
- `stages/08_UI_WINDOWING/README.md`
- `stages/09_POE_CORE/README.md`
- `stages/09_Polish_Release/ACCEPTANCE.md`
- `stages/09_Polish_Release/OVERVIEW.md`
- `stages/09_Polish_Release/TASKS.md`
- `stages/10_ONLINE_LIVEOPS/README.md`

Действия:
- Работать **строго по этапам**: нельзя начинать PoE, пока не готов UI Windowing и Modifier Algebra v2.
- Каждый этап имеет:
  - `OVERVIEW.md` — что строим
  - `TASKS.md` — что сделать
  - `ACCEPTANCE.md` — как проверить “готово”
  - `README.md` (где есть) — усиленные правила/ссылки

---

### 1.5 Assets/ (скелет кода-эталона)
- `Assets/Game/Runtime/Domain/Rng/IRng.cs`
- `Assets/Game/Runtime/Domain/Rng/XorShift32Rng.cs`
- `Assets/Game/Runtime/Domain/Stats/Modifier.cs`
- `Assets/Game/Runtime/Domain/Stats/ModifierOp.cs`
- `Assets/Game/Runtime/Domain/Stats/StatId.cs`
- `Assets/Game/Runtime/Domain/Stats/StatSheet.cs`
- `Assets/Game/Runtime/Domain/Tags/TagId.cs`
- `Assets/Game/Runtime/Domain/Tags/TagSet.cs`
- `Assets/Game/Tests/EditMode/RngDeterminismTests.cs`
- `Assets/Game/Tests/EditMode/TagSetTests.cs`

Действия:
- Эти файлы — **референс-скелет** домена (RNG/Tags/Stats + тесты).
- Если в вашем Unity проекте структуры ещё нет — создать её по `stages/07_AAA_FOUNDATION` и `specs/AAA_ARCHITECTURE_CANON.md`.
- Нельзя смешивать Unity UI код с Domain.

---

## 2) ПОЛНЫЙ СПИСОК “КОНКРЕТНЫХ ДЕЙСТВИЙ” ДЛЯ CURSOR (ПО НАШЕЙ ЛОГИКЕ)

### Этап 00–01: Vision → Foundation
1) Прочитать `stages/00_Vision/*`, `stages/01_Foundation/*`.
2) Создать `asmdef` для: Domain/Application/Infrastructure/Presentation.
3) Принять зависимости (канон): Presentation→Application→Domain. Domain без Unity.

Acceptance:
- Проект компилируется.
- Domain assembly не содержит Unity references.

---

### Этап 03: UI базовый (после 02 Input)
Цель: базовые UI слои, но **не** полноценные MMO окна.

Действия:
- Прочитать `stages/03_UI/*` + `specs/UI_WINDOWS_CATALOG_L2_D3.md` + `specs/UI_INTERACTIONS_L2_D3.md`.
- Подготовить UIRoot (single canvas), EventSystem, TMP.
- Запрет: не строить каждый экран “как отдельную сцену”. Всё — окна/панели.

---

### Этап 07_AAA_FOUNDATION: детерминизм, breakdown, budgets
Действия:
- Ввести Fixed tick + RNG streams (см. `specs/DETERMINISTIC_SIMULATION_AND_REPLAYS.md`).
- Ввести Modifier Algebra v2 + DamageBreakdown (см. `specs/COMBAT_BREAKDOWN_AND_DEATH_RECAP.md`).
- Ввести PerfBudget + pools + UI batching (см. `specs/PERFORMANCE_BUDGETING_AND_POOLS.md`).
- Добавить EditMode тесты (детерминизм RNG, корректность TagSet/StatSheet).

Acceptance:
- 100 повторов тестов детерминизма проходят.
- Нет Unity зависимостей в Domain.

---

### Этап 08_UI_WINDOWING: стабильные перемещаемые окна
Действия:
- Реализовать файлы и контракты из `specs/UI_IMPLEMENTATION_PLAN_WINDOWING.md`.
- Сделать `WindowTemplate.prefab` и перевести ключевые окна на template.
- Реализовать UILayout persistence.

Acceptance:
- 20+ окон: open/close/focus/z-order/drag/save/restore.
- Drag корректен при canvas scaling.
- Модалка блокирует клики.

---

### Этап 05/06: сквозной прототип (combat/loot/save)
Действия:
- Реализовать минимальный end-to-end loop: убийство → дроп → подбор → инвентарь → сохранение.
- Loot: D3-like (см. `specs/LOOT_SYSTEM_D3.md`) + labels + ALT show, подсветка.

Acceptance:
- Прототип играет от старта до сейва.
- Дроп отображается в мире + подписи + фильтр.

---

### Этап 09_POE_CORE: ядро PoE
Действия (строго в порядке):
P0) Modifier Algebra v2 + tags + breakdown уже должны быть готовы.
P1) Poe itemization: bases/mod pools/tiers/ilvl + validators (`specs/POE_ITEM_MODS_PREFIX_SUFFIX_DETAILED.md`)
P2) sockets/links + gems + SkillBuildCompiler (`specs/POE_GEMS_LINKS_DETAILED.md`)
P3) UI:
- Socket Inspector
- Skills Panel (умения из gems)
- Passive Tree Window (`specs/POE_PASSIVE_TREE_DETAILED.md`)
- Flask Belt UI
- Craft Window (`specs/POE_CURRENCY_ACTIONS_CATALOG.md`)
P4) Currency actions engine + ledger hooks.

Acceptance:
- Gem drag&drop работает, skills появляются/исчезают корректно.
- Supports применяются только внутри link group, и UI показывает причины отклонения.
- Currency actions детерминированы по seed и пишутся в ledger.

---

### Этап 10_ONLINE_LIVEOPS (online-ready)
Действия:
- Interest management + snapshots/deltas (`specs/NETWORK_REPLICATION_AAA.md`) хотя бы для локального SimServer.
- Persistence schema migrations (`specs/PERSISTENCE_SCHEMA_MIGRATIONS.md`)
- Observability (logs/metrics/traces), release channels, rollback.

Acceptance:
- SimServer local реплицирует состояние для UI.
- rollback hotfix данных возможен без потери профиля.

---

## 3) Чек-лист “ничего не забыто”
- UI: windowing + layout persistence + focus/modal + dragdrop service + virtualization + refresh scheduler.
- D3: loot loop (drop generation + labels + highlight + pickup + salvage + compare).
- L2: окна/соц/чат/party/clan/торговля (в UI каталоге).
- PoE: gems/links/compiler + passive tree + prefix/suffix + currency actions + flasks + PoE math.
- AAA: слои + транзакции + детерминизм + data pipeline + валидаторы + perf budgets + observability.

---

## 4) Где смотреть детализацию полей/алгоритмов
- Gems/Supports/Links: `specs/POE_GEMS_LINKS_DETAILED.md`
- Passive Tree: `specs/POE_PASSIVE_TREE_DETAILED.md`
- Prefix/Suffix/Tiers/ilvl: `specs/POE_ITEM_MODS_PREFIX_SUFFIX_DETAILED.md`
- Currency Actions: `specs/POE_CURRENCY_ACTIONS_CATALOG.md`
- Windowing: `specs/UI_WINDOWING_FRAMEWORK.md` + `specs/UI_IMPLEMENTATION_PLAN_WINDOWING.md`
- AAA: `specs/AAA_ARCHITECTURE_CANON.md` и связанные.

