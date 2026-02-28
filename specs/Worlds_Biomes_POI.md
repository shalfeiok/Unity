# Worlds: multiple worlds, biomes, deterministic spawns, POI


## Цели
- Минимум 2 мира (World_A и World_B) с порталами.
- Seed определяет план спавнов: монстры, сундуки, POI.
- World diff сохраняет изменения: открытые сундуки, убитые уникальные.

## Data
### WorldDefinition (SO)
- WorldId
- Seed
- BiomeRulesetId
- SpawnParams (density, max enemies, poi count)
- Portal links (to other worlds)
- NavMesh settings (presentation)

### BiomeRuleset (SO)
- EnemyPools (weighted)
- LootPools (weighted)
- ElementalModifiers (например +fire damage taken)
- POI pools

### POI definition
- Id, Type (camp/chest/miniboss)
- Spawn rules (min distance, required terrain tags)

## Deterministic spawn planner (Domain)
Интерфейс:
- `IWorldSpawnPlanner`:
  - `Plan(seed, worldDef, biomeRuleset) -> WorldSpawnPlan`
WorldSpawnPlan:
- list of spawn entries:
  - `SpawnId` (stable)
  - type (enemy/chest/poi)
  - position (или position hint + runtime sampling)
  - payload (enemyId, chestId, etc)
Важно:
- если position зависит от NavMesh sampling → использовать deterministic fallback (grid + nearest valid).

## World diff
- `WorldDiff`:
  - `HashSet<SpawnId> Removed` (killed uniques, opened chests)
  - `CompletedPoiIds`
Apply:
- `ApplyDiff(plan, diff) -> effectivePlan`

## Portals
- PortalDefinition links (A→B, B→A)
- Save stores current world id and last position.

## Тесты
- plan deterministic by seed
- diff apply works
