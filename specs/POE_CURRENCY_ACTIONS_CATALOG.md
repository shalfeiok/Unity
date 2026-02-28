# POE_CURRENCY_ACTIONS_CATALOG.md (операции ядра)

Дата: 2026-02-27

Цель: каталог currency actions как **операций трансформации**, без брендовых названий.

---

## 1) CurrencyAction framework (канон)
Action = {"TargetFilter","Preconditions","Transform","Postconditions","Determinism","Ledger"}

### 1.1 Общие поля
- `ActionId`
- `TargetFilter`:
  - item categories/slots
  - flags: corrupted allowed?
  - profile: PoeLike/Hybrid only?
- `Preconditions`:
  - has free prefix slot?
  - has sockets?
  - not locked?
- `Transform`:
  - mutation spec (см. ниже)
- `Determinism`:
  - uses RNG stream `Loot/Craft`
  - must be repeatable for same seed in test mode
- `Ledger`:
  - txId, actionId, itemGuid before/after hashes

---

## 2) Каталог операций (MVP)
### 2.1 Explicit mods
A1) RerollExplicitMods
- Replace all explicit mods with new roll respecting ilvl/caps/groups.

A2) AddRandomPrefix
- Add one prefix if free prefix slot.

A3) AddRandomSuffix
- Add one suffix if free suffix slot.

A4) RemoveRandomExplicit
- Remove one random explicit mod (prefix or suffix), with bias option.

A5) ReforgeWithBias(tag)
- Reroll explicit mods with increased weight for mods containing `tag`.

A6) UpgradeModTierRandomly (optional)
- Choose one explicit mod and reroll to a better tier if ilvl allows.

### 2.2 Sockets/Links/Colors
B1) SetSocketCount(count)
- Clamp to base max. Rebuild links accordingly.

B2) RerollSocketColors
- Re-roll colors with weighting from requirements.

B3) RerollLinks
- Re-roll link grouping; ensure valid partition.

B4) AddSocketIfPossible (optional)
- Increase socket count by 1 up to max.

### 2.3 Quality
C1) ImproveQuality(amount)
- Increase quality up to cap; apply via Modifier v2.

### 2.4 Corruption (optional MVP*)
D1) Corrupt
- Set corrupted flag.
- Apply one outcome from weighted set:
  - change implicit
  - change sockets/links
  - modify gem level/quality if gem present
  - no change
- After corruption: many actions disallowed.

---

## 3) UI (Craft Window)
- slot Target item
- list Actions available (filtered)
- preview “что изменится” (категории, не точные числа для reroll)
- confirm for irreversible (corrupt)
- apply → Tx + ledger entry

---

## 4) Тесты
- deterministic action with fixed seed
- preconditions enforced
- corruption blocks disallowed actions
- ledger записи создаются
