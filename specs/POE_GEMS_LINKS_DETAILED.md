# POE_GEMS_LINKS_DETAILED.md (атомарная спецификация)

Дата: 2026-02-27

Цель: детально описать ядро **Gems + Supports + Sockets/Links** так, чтобы реализация была однозначной.

---

## 0) Glossary
- **Socket**: слот в предмете с цветом.
- **Link Group**: группа сокетов, соединённых между собой (supports действуют только внутри группы).
- **Skill Gem**: активное умение.
- **Support Gem**: модификатор активного умения.
- **CompiledSkill**: результат компиляции skill gem + supports + источники статов.

---

## 1) Data (authoring → runtime)
### 1.1 SkillGemDefinition (authoring)
Поля (минимум):
- `GemId` (string/Guid key)
- `NameKey` (LocKey)
- `Tags[]` (attack/spell/projectile/aoe/minion/duration/trigger…)
- `LevelMax`
- `LevelCurve[]` (index 1..LevelMax):
  - baseDamage (в терминах DamageSpec)
  - castTime/attackTime
  - manaCost модель (flat + multipliers)
  - cooldown (optional)
  - requirements (Str/Dex/Int)
- `QualityBonuses[]`:
  - qualityStatId + deltaPer1Quality (или другой формат)
- `Payload` (что умение делает):
  - `DamageSpec` (тип урона, base, scaling tags)
  - `TargetingSpec` (self/area/point/targeted)
  - `DeliverySpec` (melee, projectile, chain, nova)
  - `AilmentSpec` (если применяет)
  - `SummonSpec` (если призывает)

### 1.2 SupportGemDefinition (authoring)
Поля:
- `GemId`, `NameKey`
- `RequiredTags[]`, `ForbiddenTags[]`
- `SupportsOnlyIf`: ruleset (например: mustHaveTag PROJECTILE)
- `Effects[]` (SupportEffectDefinition)
- `ManaMultiplier` (например 1.3)
- `DamageMultipliers`:
  - increased/reduced bucket
  - more/less bucket
- `Restrictions`:
  - cannot support triggered skills
  - cannot support minions (пример)

### 1.3 SupportEffectDefinition (каталог эффектов)
Каждый эффект имеет:
- `EffectKind` (enum, см. раздел 4)
- `Parameters` (словарь key→value)
- `AppliesTo` (scope tags)
- `Order` (для стабильного применения, детерминизм)

### 1.4 GemInstance (runtime state)
- `GemId`
- `Level`
- `Xp`
- `Quality`
- `Flags`: corrupted, locked
- `SocketRef` (itemGuid + socketIndex) если вставлен

### 1.5 Socket/Link data в ItemInstance
- `Sockets[]`: socketIndex → color
- `LinkGroups[]`: list of groups, each contains socketIndices
- `GemRefs[]`: socketIndex → GemInstanceId (или null)

---

## 2) Runtime invariants (обязательные правила)
1. В одном сокете ≤ 1 gem.
2. Skill gem активируется **только если** вставлена в предмет.
3. Supports влияют только на skill gem **внутри одной linkGroup**.
4. Support gem не усиливает другой support gem.
5. Link groups не пересекаются.
6. Если меняется link structure, compiled skills должны пересобраться.

---

## 3) Сервис: SkillBuildCompiler (канон)
### 3.1 Вход
- `EquippedItems` (или конкретный ItemInstance)
- `SocketGroupId` (linkGroup)
- `Context`:
  - player class/archetype tags
  - global modifiers (passives/items/flasks/effects)
  - seed (для детерминизма, если supports добавляют RNG)

### 3.2 Выбор основной skill gem
Правила:
- В linkGroup может быть 0..N skill gems.
- Для MVP:
  - если N==1 → она основная
  - если N>1 → игрок выбирает “primary” в UI (или “каждая создаёт отдельный CompiledSkill” — выбрать один вариант и зафиксировать).
Рекомендовано (лучшее): **каждая skill gem в группе создаёт отдельный CompiledSkill**, а supports применяются ко всем совместимым.

### 3.3 Алгоритм компиляции (порядок)
1) Старт: взять SkillGemDefinition + её уровень/качество → `SkillBase`.
2) Собрать supports в группе:
   - отфильтровать по Required/Forbidden tags (на основании текущих tags умения)
   - отсортировать по `Order`, затем по GemId (стабильная сортировка)
3) Применить supports:
   - effects, then mana multiplier
   - изменить payload (projectiles, chain, aoe, duration…)
   - обновить `SkillTags` (supports могут добавить/убрать тэги)
4) Применить глобальные модификаторы (items/passives/flasks/effects) через Modifier Algebra v2:
   - increased/reduced → суммировать
   - more/less → перемножить
   - conversion → применить по канону
5) Сгенерировать `CompiledSkill` + `Explain`:
   - список применённых supports
   - список отклонённых supports (и причина)
   - breakdown стоимости/урона/параметров

### 3.4 Выход: CompiledSkill
- `SkillId` (stable id = itemGuid + linkGroup + gemId)
- `DisplayNameKey`
- `Tags[]`
- `CostModel`
- `Cooldown`
- `DeliveryModel` (projectileCount, chainCount, areaRadius, duration, etc.)
- `DamageModel` (после conversion/gain-as-extra)
- `AilmentModel` (chance, scaling)
- `Explain`:
  - appliedSupportIds
  - rejectedSupportIds + reasons
  - numeric breakdown (DamageBreakdown + CostBreakdown)

---

## 4) Каталог SupportEffectKind (ядро)
Минимальный набор (категории):
### 4.1 Damage
- AddFlatDamage(element, min, max)
- IncreasedDamage(scopeTags, percent)
- MoreDamage(scopeTags, multiplier)
- ConvertDamage(from, to, percent)
- GainAsExtra(from, to, percent)
- Penetration(element, percent) (если добавляете)

### 4.2 Delivery
- AddProjectiles(count)
- AddChain(count)
- AddPierce(count)
- AddFork(count)
- AddSplit(count)
- IncreaseArea(percent)
- IncreaseDuration(percent)
- RepeatCast(count, delay) (если нужно)

### 4.3 Targeting/Constraints
- RequireMelee
- RequireProjectile
- DisallowTriggered
- DisallowTotems/Minions (если есть)

### 4.4 Resource/Timing
- ManaMultiplier(multiplier)
- CooldownMultiplier(multiplier)
- CastTimeMultiplier(multiplier)

### 4.5 Utility
- AddStatusChance(status, percent)
- AddKnockback(percent, strength)

---

## 5) UI требования (Socket Inspector)
### 5.1 Инспектор предмета
- Отображать сокеты (цветные квадраты) и линии линков.
- Перетаскивание gem ↔ socket:
  - подсветка допустимых сокетов
  - запрет с понятной причиной (tooltip)
- Переключатель primary skill (если выбран вариант “одна основная”).
- Показ “Какие supports применились” для выбранной skill.

### 5.2 Skills Panel
- Список CompiledSkill, источники (item + group).
- Назначение в хотбар drag&drop.

---

## 6) Тесты
### 6.1 EditMode
- детерминированность компиляции (одинаковый набор supports → одинаковый CompiledSkill)
- supports применяются только внутри linkGroup
- отклонение support при несовместимости с reason
- стабильный порядок применения supports
- качество/уровень влияет на base согласно curve

### 6.2 PlayMode
- drag&drop gems
- появление/исчезновение умений в панели
- hotbar assignment

---

## 7) Acceptance checklist
- Любое изменение сокетов/линков/камней пересобирает compiled skills.
- UI показывает причины несовместимости supports.
- Breakdown урона и стоимости доступен в tooltip.
