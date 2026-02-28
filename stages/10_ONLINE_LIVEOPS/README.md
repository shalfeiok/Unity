# Этап 10_ONLINE_LIVEOPS

Вход: `specs/NETWORK_REPLICATION_AAA.md`, `specs/PERSISTENCE_SCHEMA_MIGRATIONS.md`, `specs/OBSERVABILITY_LOGS_METRICS_TRACES.md`, `specs/RELEASE_CHANNELS_COMPAT_ROLLBACK.md`, `specs/TOOLING_GM_SIMULATORS.md`

Задачи:
1) Interest management + snapshots/deltas (локальный SimServer допустим).
2) Ledger + idempotency для операций.
3) Telemetry/logging/metrics.
4) Hotfix + flags + rollback.
5) GM/debug tools + simulators.

Acceptance:
- SimServer local реплицирует состояние в UI.
- rollback данных работает.
