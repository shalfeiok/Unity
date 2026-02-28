# POE_ITEM_MODS_PREFIX_SUFFIX_DETAILED.md

Дата: 2026-02-27

Цель: itemization PoE-стиля: базы, implicit, explicit prefix/suffix, tiers, ilvl gating, mod groups, и интеграция с D3-профилем.

---

## 1) Base items
### 1.1 ItemBaseDefinition
- BaseId, Category, Slot
- Requirements (Str/Dex/Int)
- ImplicitMods[] (Modifier v2)
- SocketRules:
  - maxSockets
  - allowedColors (по требованиям)
  - weighting (опционально)
- DropRules tags (для генератора)

### 1.2 ItemLevel (ilvl)
- ilvl приходит из источника (контент/монстр/зона).
- ilvl ограничивает доступные моды и tier.

---

## 2) Mods
### 2.1 ModDefinition
- ModId, NameKey, Kind (Prefix/Suffix/Implicit)
- GroupId (mutual exclusion)
- Tier (T1..Tn)
- RequiredItemLevel
- ApplicableTags (weapon/armor/ring, etc.)
- StatRolls[]:
  - StatId, Op(Add/Mul), Min, Max, ScopeTags
- SpawnWeight + Conditions

### 2.2 Prefix/Suffix caps
- maxPrefixes = 3
- maxSuffixes = 3
(для MVP — фиксировано, позже можно по базе)

### 2.3 Group conflicts
- нельзя иметь 2 mods с одинаковым GroupId.

---

## 3) ItemInstance (runtime)
- Guid
- BaseId
- ItemLevel
- ImplicitMods (из базы)
- ExplicitPrefixes[]
- ExplicitSuffixes[]
- Flags: corrupted, locked, junk, new

---

## 4) Генерация (PoeLike profile)
### 4.1 Choose base
- по типу источника и уровню.

### 4.2 Roll sockets/links/colors
- сначала socketCount (0..max)
- затем colors
- затем links grouping
Детерминизм по seed.

### 4.3 Roll explicit mods
- определить желаемое количество модов (по “редкости”)
- выбирать prefix/suffix с учётом:
  - ilvl gating
  - applicable tags
  - group conflicts
  - spawn weights
- затем роллить значения Min..Max.

### 4.4 Naming
- имя строить через шаблоны:
  - base name + (optional) prefix/suffix display (для UI), но не обязательно создавать “рандомные имена” как D3, можно оставить PoE-стиль “Base + моды в тултипе”.

---

## 5) Интеграция с D3-стилем
- `ItemizationProfile`: DiabloLike | PoeLike | Hybrid
- Tooltip показывает:
  - implicit
  - explicit prefixes/suffixes
  - sockets/links
  - D3-легендарные силы (если профиль Hybrid)

---

## 6) Валидаторы
- ModPoolValidator:
  - mod conflicts
  - tier gating integrity
  - weights not all zero
  - missing loc keys

---

## 7) Тесты
- генерация детерминирована (seed)
- caps соблюдаются
- group conflicts соблюдаются
- ilvl gating соблюдается
