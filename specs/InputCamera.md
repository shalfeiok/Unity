# Input, Camera, Targeting (Lineage 2 feel)


## Цели UX
- ЛКМ: контекстный клик (земля → move, враг → target, интерактив → interact).
- ПКМ удержание: orbit camera yaw/pitch (clamp pitch), плавность.
- Колесо: zoom in/out (min/max), smoothing.
- Камера: коллизия (spherecast), не проходит сквозь геометрию.
- UI приоритет: клики по UI **не** должны воздействовать на мир (InputRouter + Raycaster).

## Архитектура (Presentation слой)
### Компоненты
1) `InputRouter` (MonoBehaviour)
   - Ответственность: собрать сырой ввод (новый Input System или старый), определить контекст (UI vs World), диспатчить события.
   - Публичные события:
     - `OnWorldClick(Vector2 screenPos)`
     - `OnWorldRightDrag(Vector2 delta)`
     - `OnZoom(float scrollDelta)`
     - `OnHotbarKey(int slotIndex)`
     - `OnToggleWindow(WindowId id)`
   - Правила:
     - Если `EventSystem.current.IsPointerOverGameObject()` → world click блокируется.
     - Применяется debounce/threshold для drag.

2) `WorldRaycaster`
   - Функции:
     - `TryRaycastGround(screenPos, out RaycastHit hit)`
     - `TryRaycastTargetable(screenPos, out ITargetable target)`
     - `TryRaycastInteractable(screenPos, out IInteractable interactable)`
   - Параметры: layer masks (Ground, Targetable, Interactable).

3) `PlayerCommandController`
   - Принимает команды:
     - `MoveTo(Vector3 worldPos)`
     - `SetTarget(ITargetable target)`
     - `Interact(IInteractable obj)`
     - `RequestCast(AbilitySlot slot)` (через Application)
   - Правила:
     - авто-подход: если cast вне range → enqueue “approach then cast”
     - очередь каста: 1 слот (настройка), drop/replace политика
     - stop-on-cast: настройка
     - GCD: настройка

4) `OrbitCameraController`
   - Функции:
     - `ApplyYawPitch(deltaX, deltaY)`
     - `ApplyZoom(scroll)`
     - `ResolveCollision()`
   - Настройки:
     - sensitivity, pitchMin/pitchMax, zoomMin/zoomMax, zoomSpeed, smoothTime
     - collisionRadius, collisionMask

5) `NavMeshMoveController`
   - Обёртка над `NavMeshAgent`.
   - Функции:
     - `SetDestination(Vector3 pos)`
     - `Stop()`
     - `HasReachedDestination(stoppingDistanceEps)`
     - `Face(Vector3 point)`

## Состояния управления (мини FSM)
- `FreeMove`: клик по земле.
- `Targeted`: цель выбрана; клик по земле — сброс или move (настройка).
- `Casting`: во время каста возможна блокировка движения (stop-on-cast).
- `ApproachToCast`: движение к цели до `range`, затем каст.

## Контракты (Domain/Application)
- `CastRequest` включает:
  - `AbilityId`, `TargetRef`, `RequestedAtTime`, `QueuePolicy`, `StopOnCast`
- `CastResult`:
  - `Accepted/Rejected` + причина (cooldown, no mana, invalid target, out of range).

## Edge cases (обязательные)
- UI drag (инвентарь) не должен двигать персонажа.
- Если цель умерла во время подхода — команда каста отменяется.
- Если игрок кликает по земле во время `ApproachToCast` — отмена или переопределение (настройка).
- Если камера упёрлась в стену — корректная позиция без jitter.

## Тесты (минимум)
- Domain тесты: очередь каста (политики), правила авто-подхода/отмены.
- PlayMode (по желанию): камера collision.
