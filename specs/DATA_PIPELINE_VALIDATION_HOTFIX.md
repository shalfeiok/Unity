# DATA_PIPELINE_VALIDATION_HOTFIX.md

Дата: 2026-02-27

## Authoring → Runtime
SO как authoring, на старте/в сборке → runtime tables + индексы.

## Validators (Editor + CI)
- LootTableValidator
- ModPoolValidator (PoE prefix/suffix/tiers/group conflicts)
- SkillGemValidator (tags, supports rules)
- PassiveTreeValidator (cycles/reachability)
- LocalizationValidator

## Hotfix + flags
IHotfixSource + signature verification + rollback, feature flags + kill switch.

## Acceptance
CI падает при любой красной валидации.
