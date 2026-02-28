# Master Prompt для Cursor (RU) — с ссылками на этот пакет

Ты — CTO + Lead Engineer Unity 6000.3.10f1 LTS. Работай в корне Unity-проекта (`Assets/`, `Packages/`, `ProjectSettings/`).

## Твоя задача
1) Прочитать весь пакет документации:
- `docs/ARCHITECTURE.md`
- `docs/GLOSSARY.md`
- `docs/PRODUCTION_SCOPE.md`
- все файлы из `specs/`
- текущий прогресс в `docs/PROGRESS.md`
- этапы в `stages/`

2) Реализовать проект по этапам, строго в порядке папок `stages/*`.
Каждый этап:
- оставляет проект запускаемым,
- обновляет `docs/PROGRESS.md`,
- добавляет/обновляет EditMode тесты для доменной логики.

3) Ограничения
- Файлы ≤ 300 строк — иначе резать.
- Domain без UnityEngine.
- Без монолитов и GodManager.
- Перформанс: 0 аллокаций в hot-path, pooling, лимиты графа.

## Критерий готовности
Игра считается готовой, когда выполняются все пункты из `docs/PRODUCTION_SCOPE.md` + smoke tests из `docs/SMOKE_TESTS.md`.

## Важно
Исходный “старый” промпт лежит в `docs/SOURCE_ORIGINAL_PROMPT.md`; используй его как трассируемость требований, но **источник истины** — этот пакет.

## Доп. покрытие (Lineage II + Diablo III)
- `specs/FEATURE_MATRIX_L2_D3.md`
- `specs/UI_WINDOWS_CATALOG_L2_D3.md`
- `specs/UI_INTERACTIONS_L2_D3.md`
- `specs/LOOT_SYSTEM_D3.md`

## Канон AAA + PoE ядро
- `specs/AAA_ARCHITECTURE_CANON.md`
- `specs/UI_WINDOWING_FRAMEWORK.md`
- `specs/POE_CORE_REQUIREMENTS.md`
- `specs/DATA_PIPELINE_VALIDATION_HOTFIX.md`
- `specs/ECONOMY_LEDGER_ANTI_DUPE.md`


## PoE детализация (читать перед реализацией)
- `specs/POE_GEMS_LINKS_DETAILED.md`
- `specs/POE_PASSIVE_TREE_DETAILED.md`
- `specs/POE_ITEM_MODS_PREFIX_SUFFIX_DETAILED.md`
- `specs/POE_CURRENCY_ACTIONS_CATALOG.md`
- `specs/UI_IMPLEMENTATION_PLAN_WINDOWING.md`


## Выполнение без интерпретаций
- `docs/ARCHIVE_ACTIONS_MAP.md`
- `docs/CURSOR_EXECUTION_SCRIPT.md`

## Контроль (галочки)
- `docs/CURSOR_CHECKLIST.md`
- `docs/DEFINITION_OF_READY_DONE.md`
- `docs/STAGE_ACCEPTANCE_MATRIX.md`
