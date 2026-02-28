# ARCHITECTURE_OVERVIEW

## Слои (текущий snapshot)
1. **Domain** (`Assets/Game/Runtime/Domain`)
   - deterministic RNG streams, stat/modifier algebra, combat breakdown, PoE-модули (gems/links, passives, crafting, items).
2. **Application** (`Assets/Game/Runtime/Application`)
   - transaction-safe use cases, event publication, UI-facing orchestration.
3. **Infrastructure** (`Assets/Game/Runtime/Infrastructure`)
   - persistence, ledger, data validators, perf budget providers.
4. **Presentation** (`Assets/Game/Runtime/Presentation`)
   - windowing framework, HUD/services, tooltip/localization, input helpers (`UIInputRouter`, hotkey resolver/bindings, ESC back-navigation contract, window stack back-nav impl).
5. **Tests** (`Assets/Game/Tests`)
   - EditMode и PlayMode smoke/regression покрытие.

## Ключевые инженерные свойства
- Детерминизм: изоляция RNG по stream id + seed derivation.
- Транзакционность: повторно-идемпотентные операции через `TransactionRunner`.
- Объяснимость: breakdown/tooltip pipeline для combat и UI.
- Data-driven курс: большинство balance/definitions вынесены в definition-модели и validators.
