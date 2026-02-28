# POE_CORE_REQUIREMENTS.md (ядро)

Дата: 2026-02-27

Ядро PoE, без Atlas/Maps/Leagues.

## Состав
1) Gems + Supports + Links
2) Passive Tree (graph, поиск, zoom/pan)
3) Item mods: base+implicit, explicit prefix/suffix, tiers, ilvl gating, mod groups
4) Currency crafting: операции (reroll/add/remove/sockets/links/colors/quality/corrupt*)
5) Flasks: charges + gain rules + mods
6) PoE-math: tags, increased vs more, conversion/gain-as-extra, caps/overcap
7) Trigger framework (минимум)

## SkillBuildCompiler
SkillGem + SupportGems within linked group → CompiledSkill + explain breakdown.

## Тесты
EditMode: детерминизм seed, caps, group conflicts, ilvl gating, link rules, currency ops.
PlayMode smoke: gem drag&drop, skill → hotbar, flask charges, craft apply.


## Детализация (канон реализации)
- `specs/POE_GEMS_LINKS_DETAILED.md`
- `specs/POE_PASSIVE_TREE_DETAILED.md`
- `specs/POE_ITEM_MODS_PREFIX_SUFFIX_DETAILED.md`
- `specs/POE_CURRENCY_ACTIONS_CATALOG.md`
