# Performance, QA, тестирование, CI (optional)


## Перформанс правила (hard)
- 0 аллокаций в Update горячих системах.
- Pooling обязателен: projectiles, VFX, floating text, hit markers.
- Лимиты исполнения (graph + procs) — обязательны.
- Никаких LINQ в hot-path.

## Тестирование
### EditMode (обязательные)
- Stats math (операции/бакеты/soft caps)
- Determinism:
  - item roll by seed
  - skillcraft generation by seed
- Cooldown/cost logic
- Budget validator (over budget reject)
- Anti-exploit: ICD/recursion cap

### PlayMode (желательно)
- camera collision
- click-to-move basic

## QA артефакты
- `docs/SMOKE_TESTS.md` — перед каждым релизом.
- `Reports/` — хранить отчёты симулятора и валидатора.

## CI (если есть git)
- build + run editmode tests
- basic lint (optional)
