# STAGE_ACCEPTANCE_MATRIX.md

Дата: 2026-02-27

Матрица проверок по этапам (минимум).

## 07_AAA_FOUNDATION
- Deterministic tick + rng streams (EditMode)
- Modifier Algebra v2 correctness (EditMode)
- DamageBreakdown available (EditMode)
- PerfBudget + pools configured (manual + profiler)

## 08_UI_WINDOWING
- Open/Close/Focus/Z-order (PlayMode)
- Drag stable at canvas scales (PlayMode)
- Layout save/restore (PlayMode)
- Modal blocks clicks (PlayMode)
- No per-widget Update loops (code review)

## 09_POE_CORE
- Item generator: caps/groups/ilvl gating + deterministic (EditMode)
- SkillBuildCompiler: linkGroup-only supports + reasons + order (EditMode)
- Passive Tree allocation/respec connectivity (EditMode)
- Currency actions deterministic + ledger entries (EditMode)
- UI smoke: gem dnd, allocate node, craft apply, flask use (PlayMode)

## 10_ONLINE_LIVEOPS (когда дойдёте)
- Snapshot/delta structures compile
- Local SimServer replicates to UI
- Save migrations pass
- Hotfix rollback safe
