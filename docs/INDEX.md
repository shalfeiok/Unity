# Навигация

## 1) Что делать по шагам
- `stages/00_Vision/` — видение, pillars, определения "флагманского уровня"
- `stages/01_Foundation/` — структура проекта, архитектура, settings, документация, тестовый каркас
- `stages/02_Movement_Camera_Input/` — L2 управление, камера, роутер ввода, таргетинг, авто-подход, очередь каста
- `stages/03_UI/` — UGUI окна, hotbar, инвентарь, книга умений, крафт, таргет UI, debug overlay
- `stages/04_Domain_Core/` — доменные подсистемы: stats/combat/status/items/abilities + тесты
- `stages/05_EndToEnd_Combat_Loot_Craft/` — end-to-end петля: убил→дроп→экип→статы→крафт→скилл→каст
- `stages/06_Worlds_Portals_SaveLoad/` — миры/биомы/порталы/seed/POI + сохранения/миграции
- `stages/07_Tooling_Perf_QA_CI/` — инструменты, анти-эксплойт, профилирование, отчёты, QA, (опц) CI
- `stages/08_Content_Pack/` — стартовый контент-пак: враги/умения/предметы/миры (placeholder)
- `stages/09_Polish_Release/` — полировка, UX, баланс, стабилизация, релизные чек-листы

## 2) Подсистемные ТЗ (самые детальные)
- `specs/InputCamera.md`
- `specs/UI.md`
- `specs/Stats.md`
- `specs/Combat.md`
- `specs/Statuses.md`
- `specs/Items.md`
- `specs/Abilities_Graph.md`
- `specs/Skillcraft.md`
- `specs/Enemies_AI.md`
- `specs/Worlds_Biomes_POI.md`
- `specs/SaveLoad.md`
- `specs/VFX_Animation.md`
- `specs/Tooling_Reports.md`
- `specs/Performance_QA.md`

## 3) Архитектура и процесс
- `docs/ARCHITECTURE.md`
- `docs/GLOSSARY.md`
- `docs/PRODUCTION_SCOPE.md`
- `docs/COMPATIBILITY.md`
- `docs/PERF_BUDGET.md`
- `docs/SMOKE_TESTS.md`
- `docs/WORKFLOW.md`
- `docs/FILE_MAP.md`
- `docs/PROGRESS.md` (журнал выполнения задач)

## 4) Исходник (для трассируемости)
- `docs/SOURCE_ORIGINAL_PROMPT.md`

## Дополнения: Lineage II + Diablo III покрытие
- `specs/FEATURE_MATRIX_L2_D3.md` — полный чек-лист систем и UI-фич.
- `specs/UI_WINDOWS_CATALOG_L2_D3.md` — каталог всех окон/панелей/модалок.
- `specs/UI_INTERACTIONS_L2_D3.md` — матрица действий, drag&drop, tooltips, данные.
- `specs/LOOT_SYSTEM_D3.md` — drop/loot pipeline, подсветка, подписи на земле, фильтры.

## AAA + PoE ядро (добавлено)
### Архитектура AAA
- `specs/AAA_ARCHITECTURE_CANON.md`
- `specs/NETWORK_REPLICATION_AAA.md`
- `specs/DETERMINISTIC_SIMULATION_AND_REPLAYS.md`
- `specs/PERSISTENCE_SCHEMA_MIGRATIONS.md`
- `specs/ECONOMY_LEDGER_ANTI_DUPE.md`
- `specs/DATA_PIPELINE_VALIDATION_HOTFIX.md`
- `specs/COMBAT_BREAKDOWN_AND_DEATH_RECAP.md`
- `specs/PERFORMANCE_BUDGETING_AND_POOLS.md`
- `specs/TOOLING_GM_SIMULATORS.md`
- `specs/OBSERVABILITY_LOGS_METRICS_TRACES.md`
- `specs/RELEASE_CHANNELS_COMPAT_ROLLBACK.md`

### UI Windowing
- `specs/UI_WINDOWING_FRAMEWORK.md`

### Path of Exile ядро
- `specs/POE_CORE_REQUIREMENTS.md`

### Новые этапы разработки
- `stages/07_AAA_FOUNDATION/`
- `stages/08_UI_WINDOWING/`
- `stages/09_POE_CORE/`
- `stages/10_ONLINE_LIVEOPS/`


## PoE (детализация для реализации)
- `specs/POE_GEMS_LINKS_DETAILED.md`
- `specs/POE_PASSIVE_TREE_DETAILED.md`
- `specs/POE_ITEM_MODS_PREFIX_SUFFIX_DETAILED.md`
- `specs/POE_CURRENCY_ACTIONS_CATALOG.md`
- `specs/UI_IMPLEMENTATION_PLAN_WINDOWING.md`


## Исполнение (Cursor)
- `docs/ARCHIVE_ACTIONS_MAP.md`
- `docs/CURSOR_EXECUTION_SCRIPT.md`

## Контроль исполнения
- `docs/CURSOR_CHECKLIST.md`
- `docs/DEFINITION_OF_READY_DONE.md`
- `docs/STAGE_ACCEPTANCE_MATRIX.md`
