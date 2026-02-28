# SUMMARY_IMPLEMENTATION_REPORT

## Что добавлено в этом обновлении
- Добавлена production-реализация `WindowStackBackNavigation`, которая ведёт стек открытых окон и закрывает верхнюю панель по Esc.
- Добавлен контракт back-navigation (`IUIBackNavigation`) и реализация `NullUIBackNavigation`; `UIInputRouter.TryHandleEscape()` реализует приоритет: сначала закрытие модалки, затем верхней панели.
- Добавлен production-класс `UIInputRouter` + `DefaultUIHotkeyResolver` для прямой маршрутизации клавиш в hotkey intents с учётом `InputContext` (включая блокировку при `Modal`).
- Добавлен новый PlayMode smoke тест `UITogglesSmokeTest` с покрытием UI hotkey toggle-потока через production-класс `UIHotkeyRouter` (I/C/P/S/K/O/M -> Inventory/Character/PassiveTree/Skills/Craft/Atlas).
- Введён data-driven контракт `IUIHotkeyBindings` и реализация `DefaultUIHotkeyBindings`, чтобы hotkey mapping был инъецируемым и тестируемым без правки роутера.
- Добавлен smoke сценарий «ядро vertical slice» через существующие use case и UI-сервисы:
  - назначение скилла в hotbar;
  - аллокация узлов passive tree;
  - preview + commit currency craft операции.
- Сформирован комплект enterprise-отчётов верхнего уровня:
  - `ARCHITECTURE_OVERVIEW.md`
  - `SYSTEM_DEPENDENCY_GRAPH.md`
  - `BALANCE_GUIDELINES.md`
  - `CHANGELOG.md`

## Как тестировать
1. Запустить Unity Test Runner для PlayMode тестов.
2. Убедиться, что `UITogglesSmokeTest` проходит полностью.
3. Проверить, что существующие PlayMode smoke тесты по UI и craft не регрессировали.

## Ограничения текущего snapshot
- Тесты валидируют сервисный/доменный слой и production-маршрутизацию hotkeys в window-service orchestration, но не загружают реальные Unity-сцены (`SCN_Hub`, `SCN_RiftInstance`) в рамках этого изменения.
- Полная визуальная иерархия prefab/scene из master-спеки требует отдельного asset pass в Unity Editor.
