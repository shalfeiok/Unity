# Perf Budget (целевые бюджеты и измерения)

## Цели (ориентиры)
- 60 FPS на средних ПК и актуальных консолях (ориентир), без просадок в бою.
- 0 аллокаций в hot-path (combat/status/ability exec/input).
- Пулы: projectiles, VFX, floating text, damage numbers.

## Бюджеты исполнения AbilityGraph (пример)
- max nodes executed per cast: 64
- max targets affected: 32
- max chain depth: 8
- max repeats total: 16
- recursion depth cap: 8
- proc budget/sec per entity: 10
- fuse (max events per frame per entity): 50

## Status tick scheduler
- Один глобальный scheduler на мир.
- Тики группируются по “bucket interval” (например 0.1s/0.25s/1s) для уменьшения overhead.
- DoT/HoT должны использовать fixed-step accumulator, без per-frame аллокаций.

## Как измерять
- `CombatTestScene` — baseline для профайлера.
- Метрики:
  - ms/frame,
  - GC alloc/frame,
  - количество активных статусов,
  - число событий CombatEventBus,
  - число выполненных нод графа.

## Perf checklist
- Запрещены LINQ в Update/FixedUpdate.
- Запрещены foreach по List в горячих местах, если есть риск бокса/аллоков.
- Любые string-concat в рантайме только через cached форматирование/pooled builders.
