# Skillcraft: компоненты, грамматика, preview/reroll/craft


## Цели
- Игрок “собирает” умение из компонентов (Core/Element/Modifier/Catalyst).
- Генерация умений через грамматику (правила сочетаемости + бюджеты).
- Preview показывает:
  - теги будущего умения,
  - граф (в виде дерева/списка),
  - бюджеты,
  - ожидаемый DPS (опционально, через симулятор).
- Reroll — детерминированный набор вариантов по seed (например 3 варианта).

## Компоненты
### Core (основа)
Определяет delivery:
- Projectile / Nova / Beam / MeleeStrike / Totem
Задаёт базовый граф.

### Element
- Fire/Cold/Poison/Lightning/Physical
Добавляет тег элемента + меняет статус/вфх рецепты.

### Modifier
- Chain, Pierce, Fork, MultiCast, IncreasedCrit, AddedDoT
Изменяет граф (добавляет nodes или параметры).

### Catalyst
- “ConsumeManaForPower”, “HP as cost”, “OnKill refund”, “Combo” etc.
Зачастую влияет на resource/cooldown и budgets.

## Grammar engine (Domain)
Интерфейсы:
- `ISkillGrammar`:
  - `Generate(seed, components, ruleset) -> GeneratedAbility`
- `IRuleSet`:
  - `IsValidCombination(components) -> bool`
  - `GetAllowedModifiers(context) -> list`
- `IBudgetModel`:
  - `Evaluate(GeneratedAbility) -> Budget`

GeneratedAbility включает:
- AbilityDefinition draft (data)
- AbilityGraph (domain model)
- Compatibility report (warnings/errors)
- Budget report

## Reroll
- seed step: `seed = Hash(seed, rerollIndex)`
- варианты должны быть стабильны на одинаковом seed.

## Craft (Application)
Use cases:
- `PreviewSkillCraft(request) -> preview`
- `RerollSkillCraft(request, idx) -> preview`
- `CommitSkillCraft(request, chosenVariant) -> abilityId + store into player`

## Тесты
- deterministic generation
- invalid combinations rejected
- budgets consistent
