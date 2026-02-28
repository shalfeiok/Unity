# BALANCE_GUIDELINES

## Принципы
- Любой баланс меняется через definitions/weight tables, а не через UI-магические константы.
- Любая RNG-операция должна быть привязана к stream id для воспроизводимости.
- Любая крафт-операция должна поддерживать предсказуемый preview и безопасный commit.

## Практические правила
- Проверять affix pools валидатором до включения в runtime.
- Не смешивать increased и more в одном бакете без явного semantical reason.
- При добавлении новых currency operations сразу добавлять deterministic тест.
- Для UI comparison метрик (например ΔDPS/ΔEHP) использовать только значения из stat/breakdown pipeline.
