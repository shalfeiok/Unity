# SYSTEM_DEPENDENCY_GRAPH

## High-level
- `Presentation` -> `Application` -> `Domain`
- `Infrastructure` обслуживает `Application` и `Presentation` через адаптеры (ledger/persistence/config)
- `Tests` проверяют каждый слой изолированно + e2e smoke

## Core dependency edges
- `WindowManager` -> `IWindowService` -> `WindowRegistry`
- `AssignSkillToHotbarUseCase` -> `TransactionRunner` + `HotbarAssignmentService`
- `ApplyCurrencyActionUseCase` -> `CurrencyActionEngine` + `TransactionLedger`
- `CraftWindowService` -> `ApplyCurrencyActionUseCase`
- `AllocatePassiveNodeUseCase` -> `PassiveTreeService`

## Determinism edges
- `RngProvider` -> `IRng` per `RngStreamId`
- Loot/Craft/Skill flows используют отдельные RNG-streams
