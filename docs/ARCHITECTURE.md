# Архитектура (Clean Architecture + Unity Presentation)

## Цели архитектуры
- Доменные правила тестируемы **без Unity**.
- Умения/предметы/генерация детерминированы и воспроизводимы.
- Подсистемы расширяемы, без монолитов и GodManager.
- Перформанс: отсутствие аллокаций в hot-path, pooling, лимиты исполнения.

## Слои
### 1) Domain (pure C#)
**Запрещено:** `UnityEngine`, `MonoBehaviour`, `ScriptableObject`, `GameObject`.
**Разрешено:** структуры, математика, правила, интерфейсы, доменные события, исключения.

Папка: `Assets/Game/Runtime/Domain/`

Примеры модулей:
- Stats (StatId, Modifier, Buckets, SoftCaps)
- Combat (DamagePacket, Resolver, ProcGuards)
- Statuses (StatusDefinition runtime model, TickScheduler contracts)
- Items (Item, Affixes, Budgets, Deterministic roll)
- Abilities (AbilityGraph, Executor contracts, Limits)
- Worlds (Seeded spawn planning contracts)
- SaveModel (DTO схемы)

### 2) Application (use cases)
Оркестрация домена: “экипировать предмет”, “каст умение”, “сгенерировать лут”, “сохранить игру”.
Зависит от Domain. Не зависит от Unity.

Папка: `Assets/Game/Runtime/Application/`

### 3) Presentation (Unity)
Сцены, MonoBehaviour, UI, Input, NavMesh, анимации и VFX.
Зависит от Application/Domain, но **не наоборот**.

Папка: `Assets/Game/Runtime/Presentation/`

### 4) Infrastructure
Адаптеры: сохранения на диск, сериализация, Unity Addressables (если позже), логирование, профилирование.
Зависит от Application/Domain.

Папка: `Assets/Game/Runtime/Infrastructure/`

## Dependency Rules
- Domain ничего не знает о Unity и UI.
- Presentation “дергает” Application (use cases), а не напрямую доменную кашу.
- Все "настройки" (ScriptableObject) мапятся в Domain-конфиги через Application.

## Ключевые паттерны
- **EventBus** в Domain (без Unity), адаптируется Presentation-слоем для визуализации.
- **Composition Root** в сцене: собираем зависимости и создаём сервисы (без ServiceLocator в Domain).
- **View + Presenter/ViewModel** для UI: View занимается только отображением и вводом.

## Ограничение размера файлов
Любой файл > 300 строк обязан быть разрезан на несколько: "одна ответственность — один файл".
