# PERSISTENCE_SCHEMA_MIGRATIONS.md

Дата: 2026-02-27

## Профили
- AccountProfile
- CharacterProfile
- (опц.) SeasonProfile
- (опц.) ClanProfile

## Транзакционная запись
Commit только в конце Tx, idempotency keys.

## Миграции
schemaVersion + ISaveMigration chain.

## Acceptance
Миграция N-2→N без потери предметов/пассивов/гемов/настроек UI.
