# CTO-промпт для Cursor (RU) — Полный Production ARPG Framework на Unity  
**(Lineage 2 управление + Diablo itemization + Skillcraft + несколько миров + монстры/боссы + инструменты + перфоманс + QA/CI + правила разработки)**

> **Контекст:** мы работаем **не в репозитории**, а **в корне Unity-проекта** (папка с `Assets/`, `Packages/`, `ProjectSettings/`).  
> **Цель:** получить **production-ready каркас ARPG** (реальную платформу для игры), а не MVP-демку.  
> **Принцип:** “бесконечные вариации” = **детерминированная генерация** из модулей/правил + бюджеты + рецепты VFX/анимации, а не генерация новых ассетов “из воздуха”.

---

## 0) СУПЕР-ВАЖНО: как Cursor должен работать (режим CTO)

- Работай **поэтапно**. Каждый этап оставляет проект **запускаемым**.
- После каждого этапа/задачи:
  1) обновляй `docs/PROGRESS.md` (что сделано, какие файлы, как проверить),
  2) не допускай ошибок в Console,
  3) добавляй/обновляй тесты (EditMode) для доменной логики.
- **Файлы ≤ 300 строк**. Если больше — разбивай на меньшие по ответственности.
- **Без монолитов** и “GodManager”.
- **ООП + SOLID**: SRP/ISP/DIP обязательны.
- **Domain слой без UnityEngine**. Любая математика, генерация, правила, бюджеты — тестируемы без Unity.
- Все настройки — в **ScriptableObject Settings** (отдельно по подсистемам).

---

## 1) ГЛОССАРИЙ ТЕРМИНОВ (чтобы не было недопониманий)

**Tag / Тег** — строковый или ID-идентификатор смысла/категории: `Element.Fire`, `Delivery.Projectile`, `Status.Burn`, `Weapon.Sword`.  
**TagSet** — множество тегов (эффективная структура) с быстрыми проверками `Contains/Union`.  
**Stat / Стат** — числовой параметр (урон, крит, скорость каста, резисты).  
**Modifier / Модификатор** — правило изменения стата: `AddFlat`, `AddPercent`, `Multiply`, `Override`.  
**Bucket / Бакет** — “ведро” для сложения модов (чтобы мультипликаторы не взрывались): additive vs multiplicative.  
**Soft Cap / Мягкий кап** — формула уменьшения отдачи (diminishing returns) у статов (crit, CDR, resists).  
**Affix / Аффикс** — мод на предмете (префикс/суффикс), может давать stat modifiers и/или mutators.  
**Aspect/Legendary** — “легендарная сила” (изменяет механику, часто через mutator).  
**AbilityDefinition** — data-описание умения (теги, кд, стоимость, граф).  
**AbilityInstance** — runtime-состояние умения (остаток кд, вычисленные параметры).  
**AbilityGraph** — граф узлов (nodes), описывает выполнение умения (target query → damage → status → vfx).  
**Node / Узел** — элемент графа (Damage, ApplyStatus, Chain, Repeat, Conditional…).  
**AbilityMutator** — правило, которое **модифицирует умение/граф** по тегам (обычно от предмета/аффикса).  
**Seed** — число для детерминизма генерации.  
**IRng** — интерфейс ГСЧ (один на всё), который работает по seed.  
**Budget** — бюджеты Power/Complexity/VisualCost (чтобы генерация не ломала баланс/перфоманс).  
**WorldDefinition** — описание мира (id, seed, biome ruleset, параметры спавна).  
**BiomeRuleset** — правила биома (пулы монстров, пулы лута, усиления элементов).  
**World Diff** — “разница” мира относительно seed (что убито/открыто).

---

## 2) PROMPT ДЛЯ CURSOR (RU, CTO-уровень) — КОПИРУЙ/ВСТАВЛЯЙ ЦЕЛИКОМ

> Вставь блок ниже в Cursor Agent. Он должен **выполнить всё в корне Unity-проекта**.

```text
Ты — CTO + Lead Engineer проекта на Unity (C#). Нужно реализовать production-ready ARPG framework и базовую игру на нём.

ВАЖНО: мы работаем в корне Unity-проекта (Assets/, Packages/, ProjectSettings/). Создавай/изменяй файлы прямо там. Если нет папки docs/ — создай.

ЖАНР И ТРЕБОВАНИЯ (ОБЯЗАТЕЛЬНО):
1) Управление как Lineage 2:
   - Third-person персонаж
   - Click-to-move: ЛКМ по земле → NavMeshAgent.SetDestination
   - Таргетинг: ЛКМ по врагу/объекту → выбрать цель
   - Камера: удержание ПКМ → orbit yaw/pitch вокруг персонажа с ограничением pitch
   - Колесо мыши → zoom (min/max)
   - Камера с коллизией (spherecast), не проходит через стены
   - UI ввод не должен вызывать перемещение/таргет (InputRouter приоритеты)
   - Auto-approach при касте вне дистанции (подойти и кастануть)
   - Stop-on-cast (опционально по настройке)
   - Cast queue (минимум 1 слот очереди)
   - Global cooldown (опционально по настройке)
2) UI (UGUI):
   - Hotbar (10 слотов) + бинды 1..0 + клик по слоту кастит
   - InventoryWindow (I): сетка предметов + tooltip
   - Character/StatusWindow (C): статы/ресурсы
   - SkillBookWindow (K): список умений
   - CraftingWindow: выбор компонентов → preview → reroll → craft
   - Target UI (минимум): имя/HP цели
   - Debug overlay (dev): теги/моды/статусы/счётчики событий
3) ARPG системы (полный набор):
   A) Stat System:
      - статы: primary/offense/defense/utility
      - модификаторы: AddFlat/AddPercent/Multiply/Override
      - бакеты и soft caps
      - временные модификаторы (бафы/ауры)
   B) Combat:
      - DamagePacket, DamageResolver (элементы, резисты, крит)
      - ресурсы HP/MP/(опц. Stamina)
      - CombatEventBus (OnHit/OnCrit/OnKill/OnCast/OnTakeDamage…)
      - anti-exploit: ICD, proc budgets/sec, recursion depth, fuse limits
   C) Status System:
      - DoT/HoT/Slow/Stun/Freeze/Poison + расширяемость
      - правила стаков + оптимизированный tick scheduler
   D) Itemization (Diablo-like):
      - ItemArchetype, Rarity, AffixDefinition (prefix/suffix pools)
      - Legendary/Aspect powers
      - конфликты аффиксов + ограничения
      - детерминированный ролл по seed
      - Item power budget + tradeoffs
      - генерация имён и tooltip breakdown
   E) Ability System:
      - AbilityDefinition/AbilityInstance
      - cooldown/cost/channel (support-ready)
      - AbilityGraph (минимум MVP узлы + расширяемый список до 30+)
      - execution limits (max nodes/targets/chains/repeats)
      - Budget evaluator: Power/Complexity/VisualCost
      - AbilityMutators: предметы/аффиксы модифицируют умения по TagSet
   F) Skillcraft:
      - компоненты: Core/Element/Modifier/Catalyst
      - grammar-based генератор умений
      - preview с бюджетами + reroll + craft
      - умение в SkillBook, затем на Hotbar
   G) Enemies:
      - EnemyDefinition + prefab
      - AI FSM: Idle/Patrol/Aggro/Chase/Attack/Flee/Dead
      - elites и boss support-ready (модификаторы, механики)
      - DropTable + XP reward + scaling by world
   H) Worlds:
      - WorldDefinition + BiomeRuleset
      - несколько миров (минимум World_A и World_B) и порталы
      - детерминированные спавны по seed
      - POI support-ready (лагеря/сундуки/мини-боссы)
   I) Save/Load:
      - сохранять: player, inventory, equipped, skill seeds, world id
      - world diff: opened chests, killed uniques (минимум)
      - generatorVersion + migration-ready hooks
4) VFX/Animation pipeline:
   - VFX recipe resolver по TagSet (shape modules + element overlays), placeholders допустимы
   - pooling VFX
   - Animation recipes: MotionIntent/Delivery → верхний слой/аддитив/IK toggle
   - интеграция событий каста → VFX/Anim
5) Tooling (ОБЯЗАТЕЛЬНО):
   - AbilityGraph inspector/editor (минимум: просмотр, валидация, отчёт)
   - DPS simulator (export CSV/JSON в Reports/)
   - Balance/Budget report генерация
   - Tag explorer + Event monitor overlay
6) Performance rules (ОБЯЗАТЕЛЬНО):
   - 0 аллокаций в hot Update путях (combat/status/ability exec)
   - pooling: projectiles/VFX/floating text
   - scheduler для статусов
   - лимиты исполнения графа
   - baseline profiler сцена и документ perf budget
7) Качество/процесс (ОБЯЗАТЕЛЬНО):
   - Clean architecture: Domain/Application/Presentation/Infrastructure
   - Domain без UnityEngine
   - ООП+SOLID
   - файлы ≤ 300 строк
   - все настройки в ScriptableObject Settings (отдельно)
   - документация: docs/ARCHITECTURE.md, docs/GLOSSARY.md, docs/PRODUCTION_SCOPE.md, docs/COMPATIBILITY.md
   - прогресс: docs/PROGRESS.md (после каждой задачи)
   - тесты EditMode для доменной логики:
     - стат-математика
     - детерминизм item/ability generation
     - cooldown/cost
     - budget validator
     - anti-exploit guards (минимум: ICD/recursion cap)
   - никакие этапы не оставляют проект сломанным

ДОПОЛНИТЕЛЬНЫЕ ТРЕБОВАНИЯ К СТРУКТУРЕ:
- Создай папки:
  - Assets/Game/Runtime/{Domain,Application,Presentation,Infrastructure}
  - Assets/Game/Data/{Settings,Items,Abilities,Statuses,Worlds}
  - Assets/Game/Scenes
  - Assets/Game/Prefabs
  - Assets/Game/Tests (EditMode)
  - docs/
  - Reports/
- Создай сцены:
  - MainTestScene (core loop)
  - CombatTestScene (sandbox)
  - World_A, World_B (переходы)
- Не используй внешние ассеты. Всё — primitives/placeholder.

ПОРЯДОК РЕАЛИЗАЦИИ:
1) Основа: структура + Settings + docs + прогресс + (опц) CI конфиг под git, но не обязательно если нет репозитория
2) Управление/камера
3) UI окна + hotbar
4) Domain: stats/items/combat/status/abilities + tests
5) End-to-end loop: kill → drop → equip → stats → craft → skill → cast
6) Worlds + portals + save/load
7) Tooling + anti-exploit hardening + performance pass

После каждого шага:
- обнови docs/PROGRESS.md: что сделано, файлы, тесты, ручная проверка
- убедись, что сцены запускаются без ошибок

Это production framework. Делай расширяемо и безопасно.
```

---

## 3) ПОЛНЫЙ ЧЕК-ЛИСТ ПУНКТОВ (ПРОВЕРКА НАЛИЧИЯ)

Ниже — контрольный список “всё из жанра + всё из требований”. Cursor обязан покрыть каждый пункт.

### 3.1 Управление/камера/передвижение (Lineage 2 feel)
- [ ] Click-to-move (NavMeshAgent)  
- [ ] UI блокирует world input  
- [ ] RMB orbit camera yaw/pitch + pitch clamp  
- [ ] Mouse wheel zoom min/max + smoothing  
- [ ] Camera collision spherecast  
- [ ] Targeting по клику  
- [ ] Auto-approach при касте вне range  
- [ ] Stop-on-cast (настройка)  
- [ ] Cast queue (настройка)  
- [ ] Global cooldown (настройка)

### 3.2 UI (обязательные окна)
- [ ] Hotbar 10 слотов + 1..0 + клик  
- [ ] InventoryWindow (I) + tooltip  
- [ ] Character/StatusWindow (C)  
- [ ] SkillBookWindow (K)  
- [ ] CraftingWindow (preview/reroll/craft)  
- [ ] Target UI (name/HP)  
- [ ] Debug overlay (tags/mods/statuses/events)

### 3.3 Core systems
- [ ] StatSystem (buckets + soft caps + modifiers)  
- [ ] Combat: DamagePacket + DamageResolver (elements/resists/crit)  
- [ ] ResourceSystem (HP/MP/опц Stamina)  
- [ ] StatusSystem (DoT/HoT/Slow/Stun/Freeze/Poison)  
- [ ] Tick scheduler оптимизированный  
- [ ] CombatEventBus (OnHit/OnCrit/OnKill/OnCast/OnTakeDamage)  
- [ ] Anti-exploit: ICD + budgets/sec + recursion cap + fuse

### 3.4 Items & Loot (Diablo-like)
- [ ] ItemArchetype  
- [ ] Rarity tiers  
- [ ] AffixDefinition pools prefix/suffix  
- [ ] Affix conflicts / unique / maxPerItem  
- [ ] Legendary/Aspect system  
- [ ] Deterministic roll by seed  
- [ ] Item power budget + tradeoffs  
- [ ] Name generation  
- [ ] Tooltip breakdown (источники модов)

### 3.5 Abilities & Skillcraft
- [ ] AbilityDefinition/Instance, cooldown/cost  
- [ ] AbilityGraph + executor + context  
- [ ] Execution limits  
- [ ] Budgets Power/Complexity/VisualCost  
- [ ] AbilityMutators from items  
- [ ] Skillcraft components + grammar generator  
- [ ] Skill preview + budgets bars + reroll + craft  
- [ ] SkillBook + hotbar integration

### 3.6 Enemies / AI / Progression
- [ ] EnemyDefinition + scaling  
- [ ] AI FSM (Idle/Patrol/Aggro/Chase/Attack/Flee/Dead)  
- [ ] Elites support-ready (модификаторы)  
- [ ] Boss support-ready (механики)  
- [ ] DropTables  
- [ ] XP reward + leveling support

### 3.7 Worlds / Biomes / POI
- [ ] WorldDefinition (id, seed)  
- [ ] BiomeRuleset (pools, modifiers)  
- [ ] World_A, World_B сцены  
- [ ] Portals  
- [ ] Deterministic spawns by seed  
- [ ] POI support-ready (camps/chests/miniboss)

### 3.8 Save/Load
- [ ] Save schema: player/inventory/equipped/skills/world id  
- [ ] World diff: opened chests/killed uniques  
- [ ] generatorVersion  
- [ ] migration-ready hooks

### 3.9 VFX/Animation pipeline
- [ ] VFX recipes by tags (shape + element overlay)  
- [ ] VisualCost budget + pooling  
- [ ] Anim recipes by MotionIntent + upper-body overlays + additive + IK toggle  
- [ ] Integration: ability events → VFX/Anim

### 3.10 Tooling / QA / Perf
- [ ] AbilityGraph inspector/editor  
- [ ] DPS simulator + Reports export  
- [ ] Balance/Budget report  
- [ ] Tag explorer + event monitor  
- [ ] Perf baseline scene + docs/PERF_BUDGET.md  
- [ ] 0 alloc hot paths + pooling + scheduler  
- [ ] EditMode tests: stat math, determinism, cooldown/cost, budget validator, anti-exploit

---

## 4) Правила код-стайла (чтобы Cursor не “размазал” проект)

### 4.1 Нейминг
- Classes/Structs: PascalCase  
- Methods: PascalCase  
- private fields: `_camelCase`  
- interfaces: `IName`  
- ScriptableObjects: `SomethingSettings`, `SomethingDefinition`

### 4.2 Исключения/ошибки
- Domain: бросаем осмысленные исключения (`InvalidGraphException`, `BudgetExceededException`)  
- Presentation: ловим, логируем, показываем UI feedback, **не падаем**.

### 4.3 DI (минимум, но правильно)
- На раннем этапе допускается простой `ServiceRegistry` (composition root) **только** в Presentation/Infrastructure.
- Domain не использует Service Locator.

### 4.4 UI паттерн
- View (MonoBehaviour) + Presenter/ViewModel (pure C#).
- View подписывается на события, не вычисляет бизнес-логику.

---

## 5) Документация (обязательные файлы)

Создать в `docs/`:
- `ARCHITECTURE.md` — слои, зависимости, правила
- `GLOSSARY.md` — термины (можно из этого документа)
- `PRODUCTION_SCOPE.md` — полный scope (что поддерживается)
- `COMPATIBILITY.md` — матрица совместимости узлов/тегов/аффиксов
- `PERF_BUDGET.md` — целевые бюджеты + как измерять
- `SMOKE_TESTS.md` — ручные проверки
- `PROGRESS.md` — журнал задач

---

## 6) Сцены и минимальный “контентный набор” (чтобы не было пусто)

Cursor должен сделать “пакет стартового контента” (плейсхолдеры):

### 6.1 Миры
- World_A: равнина/лес (простая сцена с NavMesh)  
- World_B: руины/пещера (простая сцена с NavMesh)  
- Portal A→B и обратно

### 6.2 Монстры (минимум)
- 3 обычных типа (melee, ranged, fast)  
- 1 elite модификатор (например “Berserk”: speed+damage, но меньше HP)  
- 1 boss-заглушка (больше HP, простая механика: периодический AoE)

### 6.3 Умения/статусы (минимум)
- 2 умения базовых (projectile + nova)  
- 3 статуса (Burn, Slow, Poison)  
- 1 легендарный mutator пример (“OnCrit explosion”)

### 6.4 Предметы (минимум)
- 3 archetype (sword, staff, armor)  
- 10 аффиксов (damage%, crit, cast speed, resists…)  
- rarity tiers (common→legendary)

---

## 7) Главное: мы в корне Unity проекта (не git репо)

- Все пути/файлы создавай прямо в проекте:
  - `Assets/Game/...`
  - `docs/...`
  - `Reports/...`
- Если есть `.github/` и git — можно добавить CI/PR шаблоны, но **не требовать** git.
- В любом случае: реализуй “commit policy” как документ `docs/WORKFLOW.md` (даже если git нет).

---

## 8) Финальный критерий “игра считается собранной как жанр”

Игра должна позволять:
- двигаться (как L2),
- выбирать цель,
- кастовать скиллы с кд/стоимостью,
- убивать монстров,
- получать дроп,
- экипировать предметы и видеть влияние на статы,
- создавать предметы/умения через крафт,
- видеть preview умений и бюджеты,
- переходить между мирами,
- сохраняться/загружаться,
- иметь инструменты разработчика (DPS/Graph inspector),
- не ломаться при “эксплойтных” попытках (лимиты/ICD/бюджеты).

---

## 9) Приложение: шаблон записи прогресса (docs/PROGRESS.md)

Пример формата (Cursor обязан соблюдать):

### TASK B-04 — Orbit camera RMB
- Статус: DONE
- Что сделано:
  - OrbitCameraController, Pitch clamp, Settings
- Файлы:
  - Assets/Game/Runtime/Presentation/Camera/OrbitCameraController.cs
  - Assets/Game/Data/Settings/CameraSettings.asset
- Тесты:
  - (если есть) Camera math unit tests
- Ручная проверка:
  1) Запустить MainTestScene
  2) Зажать ПКМ, повернуть камеру
  3) Проверить clamp и collision
- Риски/заметки:
  - ...

---

# Конец документа
