# Enemies: definitions, AI FSM, elites, boss support-ready


## Цели
- Враги управляются данными (EnemyDefinition) + универсальный FSM.
- Поддержка модификаторов элит (affix-like) и босс механик.
- Детерминированный дроп по seed.

## EnemyDefinition (SO)
- Id, Name
- Prefab placeholder
- BaseStats
- Abilities list (ids)
- AggroRange, AttackRange, MoveSpeed
- DropTableId
- XP reward, level scaling rules
- Tags (Enemy.Undead, Enemy.Beast, etc)

## AI FSM (Presentation + minimal Domain rules)
Состояния:
- Idle
- Patrol
- Aggro (acquire target)
- Chase
- Attack (ability selection)
- Flee (optional)
- Dead

Функции:
- `UpdateState(dt)`
- `TransitionTo(state)`
- `SelectAbility(targetContext)`

Правила:
- threat/aggro: минимально — closest in range, позже можно расширить.
- attack cadence: cooldown windows, windup.

## Elites
- EliteModifierDefinition:
  - stat boosts
  - extra ability mutators
  - visual overlay (tagged)
- Правила:
  - бюджет сложности (не больше X модификаторов)
  - deterministic roll per spawn

## Boss
- BossMechanicDefinition:
  - фазность по HP
  - таймерные механики (AoE, adds spawn)
- Требования:
  - механики ограничены budgets и лимитами событий

## Тесты
- FSM transitions (domain-ish via pure state machine)
- deterministic elite roll
