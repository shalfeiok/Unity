# Save/Load: schema, versioning, migrations


## Цели
- Сохранить: player stats, inventory, equipped, skill seeds/ids, current world id, world diff.
- Версионирование: `saveVersion` и `generatorVersion`.
- Миграции: hooks для преобразования старых сохранений.

## Schema (DTO)
### SaveGameDto
- `int SaveVersion`
- `int GeneratorVersion`
- `PlayerDto Player`
- `InventoryDto Inventory`
- `EquipmentDto Equipment`
- `SkillsDto Skills`
- `WorldStateDto WorldState`
- `long SavedAtUnix`

### WorldStateDto
- `string CurrentWorldId`
- `int WorldSeed`
- `Dictionary<string, WorldDiffDto> WorldDiffs` (key=worldId)

### WorldDiffDto
- `List<string> RemovedSpawnIds`
- `List<string> OpenedChestIds` (если отдельно)
- `List<string> CompletedPoiIds`

## Serialization
- JSON (быстро внедрить) + компрессия optional.
- Файлы:
  - `Saves/save_001.json`
  - `Saves/backup/...`

## API (Infrastructure)
- `ISaveRepository`:
  - `Save(SaveGameDto dto)`
  - `TryLoad(slot) -> dto`
  - `ListSaves()`

## Migrations
- `ISaveMigration`:
  - `FromVersion`, `ToVersion`
  - `Migrate(dto) -> dto`
- registry миграций, применяется цепочкой.

## Тесты
- roundtrip serialize/deserialize
- migration chain works
