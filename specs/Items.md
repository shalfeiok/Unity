# Items & Itemization (Diablo-like): архетипы, аффиксы, аспекты, бюджеты


## Цели
- Генерация предметов на основе:
  - архетипа,
  - редкости,
  - seed,
  - ruleset биома/мира.
- Полный tooltip breakdown: какие моды откуда.
- Жёсткие ограничения: power budget + конфликты аффиксов.
- Легендарные аспекты, меняющие механику (AbilityMutator/CombatProc).

## Data definitions
### ItemArchetypeDefinition (SO)
- ItemType (Weapon/Armor/Accessory/Consumable)
- EquipSlot (MainHand/OffHand/Chest/...)
- BaseStats (list of StatBase)
- AllowedAffixPools (by rarity)
- Tags (Weapon.Sword, Armor.Heavy, ...)
- Visual placeholder info (prefab id)

### AffixDefinition (SO)
- Id, Name, Prefix/Suffix
- Tags (Element.Fire, Stat.Crit)
- Modifiers: list of ModifierTemplate (stat, op, min/max roll, bucket)
- Constraints:
  - RarityMin/Max
  - Conflicts: list of AffixId
  - MaxPerItem
  - Weight (для ролла)
- Optional:
  - AddsAbilityMutatorId / ProcId

### AspectDefinition (SO)
- Id, Name
- ApplyRules (requires tags, forbidden tags)
- Effect:
  - AbilityMutator (по тегам умений)
  - CombatProc (OnHit/OnCrit etc)
- BudgetCost (power, complexity)

## Генератор предметов (Domain/Application)
Функции:
- `RollItem(ItemRollRequest req, IRng rng) -> Item`
- `RollAffixes(Item item, req, rng) -> void`
- `ValidateItem(Item item) -> ItemValidationResult`

### ItemRollRequest
- archetypeId
- rarity
- seed
- worldId/biomeId (для пулов)
- level / itemPowerTarget

### Budgets
- Каждый affix/aspect имеет стоимость.
- Item имеет лимит по rarity/level.
- Правила tradeoff:
  - если affix дорогой → меньше слотов или снижается диапазон ролла.

## Tooltip breakdown
Модель:
- `ItemTooltipModel`:
  - Name line (rarity color)
  - Base stats section
  - Affix lines (каждая с min/max и rolled value)
  - Aspect section
  - Derived summary (DPS, armor, etc)
  - Debug seed line (dev)

## Тесты
- deterministic roll (seed → одинаковый набор affix и значения)
- conflict enforcement
- budget enforcement
- name generation deterministic
