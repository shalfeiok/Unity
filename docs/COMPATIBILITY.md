# Compatibility Matrix (теги/узлы/аффиксы/аспекты)

Этот документ обязателен, чтобы:
- избегать конфликтов аффиксов,
- валидировать AbilityGraph,
- предотвращать “слом” билдов при добавлении нового контента.

## 1) Политика совместимости
- Любая новая фича добавляет:
  1) новые теги (если нужно),
  2) правила совместимости,
  3) тесты валидации.
- Запрещены неявные зависимости “на глаз”.

## 2) Матрицы (шаблоны)
### 2.1 Node ↔ Delivery
- DamageNode требует Delivery-тег: `Delivery.Melee|Projectile|AoE|Beam`.
- ProjectileSpawnNode запрещён с `Delivery.Melee` без конвертера.

### 2.2 Status ↔ Element
- `Status.Burn` требует `Element.Fire`.
- `Status.Freeze` требует `Element.Cold`.
- `Status.Poison` запрещён с `Element.Cold` (пример правила) — настраивается.

### 2.3 Affix conflicts
- `Affix.CritChance` конфликтует с `Affix.CritChance_HighTier` (mutual exclusive).
- `Affix.AttackSpeed` имеет maxPerItem = 1.
- Prefix/Suffix cap: зависит от rarity.

### 2.4 Legendary aspects ↔ Ability tags
- `Aspect.OnCritExplosion` применим только к умениям с тегом `Hit.CanCrit`.
- `Aspect.ProjectilesSplit` только к `Delivery.Projectile`.

## 3) Как обновлять
- Любая новая сущность должна быть описана:
  - какие теги добавляет,
  - к чему применяется,
  - с чем конфликтует,
  - какие лимиты/бюджеты.
