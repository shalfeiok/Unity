# Tooling: AbilityGraph inspector/editor, DPS sim, balance/budget reports


## Цели
- Инструменты разработчика: валидатор графов, симуляторы, отчёты, монитор событий.
- Экспорт отчётов в `Reports/` (CSV/JSON).
- Инструменты не должны влиять на релиз (compile defines / dev-only scene objects).

## 1) AbilityGraph Inspector/Validator
Функции:
- открыть AbilityDefinition
- показать:
  - список узлов (в порядке обхода),
  - теги,
  - лимиты,
  - budgets,
  - совместимость (из docs/COMPATIBILITY.md)
- валидация:
  - unreachable nodes,
  - cycles,
  - missing required tags,
  - cost overflow,
  - limit overflow potential.

Outputs:
- `GraphValidationReport` (JSON)

## 2) DPS simulator
Вход:
- AbilityDefinition + StatSheet snapshot + target profile
- Simulation settings: duration, sample count, rng seed
Выход:
- avg DPS, variance, proc rate, status uptime
Экспорт:
- CSV summary + JSON full.

## 3) Balance/Budget report generator
- сканирует все Ability/Item definitions
- строит отчёт: power distributions, outliers
- пишет в `Reports/Balance_<date>.json`

## 4) Tag explorer + event monitor overlay
- UI overlay:
  - текущие теги игрока/цели
  - последняя сотня CombatEventBus событий
  - counters: nodes executed, procs/sec

## Требования
- все инструменты должны работать на placeholder контенте из stages/08_Content_Pack.
