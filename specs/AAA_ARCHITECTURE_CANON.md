# AAA Архитектура гибрида L2 + D3 + PoE (канон проекта)

Дата: 2026-02-27

Ключевые принципы:
- **Domain без Unity** (никаких UnityEngine/MonoBehaviour).
- **Presentation без бизнес-логики** (только рендер данных и ввод).
- Любое действие = **атомарная транзакция** (validate → apply → events → commit).
- Всё **data-driven** и валидируется (Editor + CI).
- Детерминизм: Fixed tick + RNG streams + (опц.) replay/trace.

## 1) Слои
- Domain (правила/математика/симуляция)
- Application (UseCases, Tx, события)
- Infrastructure (сеть, save, telemetry, hotfix)
- Presentation (Unity UI/World)

## 2) Зависимости (asmdef)
- Presentation → Application
- Application → Domain
- Infrastructure → (Domain/Application)
- Domain → только BCL

## 3) События
- Domain events: DamageApplied, ItemGenerated, LootSpawned…
- Application events: UiToastRequested, PlaySfxRequested…

## 4) Транзакции
Tx: Begin → Validate → Apply → Publish → Commit (+ idempotency/ledger).

## 5) Data pipeline + hotfix
SO authoring → runtime tables + индексы. Валидаторы обязательны. Hotfix + feature flags + rollback.

## 6) Perf budgets
Pools, UI batching, лимиты сущностей/labels/обновлений.

## 7) SimServer стратегия
Оффлайн строится как локальный authoritative слой, чтобы потом перенести на сервер.
