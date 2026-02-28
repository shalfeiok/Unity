# RELEASE_CHANNELS_COMPAT_ROLLBACK.md

Дата: 2026-02-27

- Каналы: dev/qa/staging/prod
- Compat window для snapshot schema
- Save migrations
- Rollback: hotfix data rollback + kill switch + safe-mode login

Acceptance: откат возвращает игру в рабочее состояние без потери профиля.
