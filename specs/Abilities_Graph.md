# Abilities: Definition/Instance + AbilityGraph + executor + mutators + limits


## Цели
- Умение описывается данными (SO) и исполняется через граф.
- Один executor для всех умений.
- Лимиты: nodes/targets/chains/repeats/recursion.
- Budget evaluator: Power/Complexity/VisualCost.
- Mutators (от предметов/аспектов) модифицируют граф по TagSet.

## Модели данных
### AbilityDefinition (SO)
- Id, Name, Icon
- Tags (Element.Fire, Delivery.Projectile, Hit.CanCrit, ...)
- Cast:
  - CastTime (0=instant)
  - Channel (optional)
  - CooldownSeconds
  - Cost (resource, amount)
  - Range, Radius
  - CanMoveWhileCasting (bool)
- GraphReference (AbilityGraph asset or embedded definition)
- BudgetBase (power/complexity/visual)

### AbilityInstance (runtime)
- AbilityId
- CooldownRemaining
- LastCastTime
- CachedComputedParams (range, damage multipliers etc) derived from stats

## AbilityGraph
### Node базовый контракт (Domain)
- `NodeId`
- `Execute(AbilityExecContext ctx, NodeExecState state) -> NodeResult`
Где:
- ctx: source/target(s), rng, time, event bus, stats, limits
- state: per-cast local state (counters, stacks)

### Минимальный набор узлов (MVP)
1) TargetQueryNode (single target, cone, radius, self)
2) DamageNode (creates DamagePacket(s))
3) ApplyStatusNode
4) SpawnProjectileNode (Presentation adapter hook)
5) ChainNode (execute child for each target)
6) RepeatNode (N times)
7) ConditionalNode (by tags, hp %, crit last hit, etc)
8) VfxNode / SfxNode (signals to Presentation, costed by VisualCost)
9) WaitNode (для channel/sequence, optional)

### Execution limits
- `AbilityExecLimits { MaxNodes, MaxTargets, MaxChainDepth, MaxRepeats, MaxRecursionDepth }`
Проверка:
- перед выполнением узла + при расширении целей/повторов.
Ошибка:
- `ExecutionLimitExceededException` (Domain)

## Mutators
- `IAbilityMutator`:
  - `bool CanApply(AbilityDefinition def, TagSet tags)`
  - `void Apply(AbilityGraph graph, MutatorContext ctx)`
Примеры:
- “Projectiles split” → вставляет node fork/extra projectiles.
- “OnCrit explosion” → добавляет conditional + AoE damage.

## Budget evaluator
- Каждому узлу присваивается стоимость:
  - power, complexity, visual
- Итог = base + nodes + mutators.
- Если превышено → craft preview красный, каст запрещён (настройка).

## Тесты
- deterministic execution with fixed seed
- limit caps
- mutator application deterministic
- budget validator
