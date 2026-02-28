# DETERMINISTIC_SIMULATION_AND_REPLAYS.md

Дата: 2026-02-27

## Fixed tick
SimTime(tickIndex, dt) + фиксированный порядок систем.

## RNG streams
RngStreamId: Combat/Loot/AI/World. Запрет UnityEngine.Random в домене.

## Replay/Trace (AAA debug)
- запись InputCommand + seeds
- воспроизведение → тот же WorldState
- breakdown урона/генерации лута

## Acceptance
- 100 повторов replay дают идентичный результат.
