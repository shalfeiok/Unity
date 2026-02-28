# Combat: DamagePacket, Resolver, ресурсы, события, анти-эксплойт


## Цели
- Единая система урона для всех источников (melee, projectile, DOT, traps).
- Расширяемость по элементам, резистам, crit, proc.
- Событийная шина для триггеров аффиксов/аспектов/статусов.
- Anti-exploit: лимиты проков, recursion cap, ICD.

## Domain модели
### DamagePacket
Поля:
- `EntityId Source`, `EntityId Target`
- `float BaseDamage`
- `DamageElement Element` (None/Fire/Cold/Poison/Lightning/Physical…)
- `TagSet Tags` (например `Hit.CanCrit`, `Delivery.Projectile`)
- `float CritChanceOverride?` (опц), `float CritMultiplierOverride?` (опц)
- `SourceRef Origin` (AbilityId/ItemId/StatusId)
- `int ChainDepth` (для ограничений)
- `uint RandomSalt` (для детерминизма в resolver)

### DamageContext
- ссылки на `StatSheet` source/target
- `ResistProfile` target
- `ProcGuard` (см. ниже)
- текущая time (для ICD)

### DamageResult
- `float FinalDamage`
- `bool IsCrit`
- `float Blocked/Absorbed`
- `AppliedStatuses[]` (если часть резолва)
- `EventsEmittedCount`

## Resolver responsibilities
Функция:
- `Resolve(DamagePacket p, DamageContext ctx) -> DamageResult`

Шаги:
1) validation (target alive, chainDepth < cap)
2) apply source offense stats (damage%, element bonus, vulnerability)
3) crit roll (deterministic rng via IRng + salt)
4) apply target defense (armor/resists, damage reduction)
5) apply shields/absorbs
6) clamp (no negative)
7) emit events:
   - `OnHit`, `OnCrit`, `OnTakeDamage`, `OnKill` (если HP ≤ 0)
8) anti-exploit checks (budget/sec, fuse)

## Resources
- `ResourceId` (HP/MP/Stamina)
- `ResourcePool`:
  - `GetCurrent/Max`
  - `TrySpend(amount) -> bool`
  - `Add(amount)`
  - `Regenerate(dt)` (через scheduler)

## CombatEventBus (Domain)
- `Publish(CombatEvent e)`
- `Subscribe<TEvent>(handler)`
События:
- OnCastStart/OnCastEnd
- OnHit/OnCrit
- OnTakeDamage
- OnKill
- OnStatusApplied/Expired

## Anti-exploit guards
### 1) ICD (internal cooldown)
- per (source, procId) store lastTime, reject if < cooldown.

### 2) Proc budget/sec
- per entity counters; if exceeded — suppress procs.

### 3) Recursion cap
- chainDepth in packet; plus global recursion depth for graph execution.

### 4) Fuse limits
- max events per frame per entity: prevents “event storms”.

## Тесты
- deterministic crit with fixed seed/salt
- resist/armor formulas
- ICD and budget suppression
- recursion cap triggers
