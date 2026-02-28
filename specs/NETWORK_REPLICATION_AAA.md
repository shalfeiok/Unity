# NETWORK_REPLICATION_AAA.md

Дата: 2026-02-27

## Interest management
- Near/Mid/Far LOD по расстоянию
- Party members всегда видимы
- Loot labels/вспомогательные сущности — отдельный канал

## Snapshot/Delta
- WorldSnapshot(tick, entities/components)
- DeltaSnapshot(changes since last ack)

## Prediction (минимум)
- движение предиктится, сервер корректирует
- интеракции/касты — optimistic UX, authoritative commit

## Версии схемы
- SnapshotCodec vX, compat window

## Acceptance
- Interest снижает bandwidth
- Delta меньше full snapshot при стабильном мире
