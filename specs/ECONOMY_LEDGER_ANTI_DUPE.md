# ECONOMY_LEDGER_ANTI_DUPE.md

Дата: 2026-02-27

## Ledger
ITransactionLedger пишет: loot pickup, craft apply, trade commit, market ops, salvage.

## Idempotency
повтор запроса не создаёт дубликат.

## Item GUID + audit trail
у каждого item GUID + ownership chain.

## Acceptance
1000 повторов pickup → 1 item в инвентаре.
