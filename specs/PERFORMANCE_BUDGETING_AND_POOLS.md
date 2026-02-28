# PERFORMANCE_BUDGETING_AND_POOLS.md

Дата: 2026-02-27

- PerfBudget(maxEnemies/maxProjectiles/maxLootLabels/maxUiRefreshPerFrame)
- Pools: projectiles, floating texts, loot actors, tooltips/toasts
- UIRefreshScheduler batching

Acceptance: без GC spikes при спаме лута/боёв (dev профилирование).
