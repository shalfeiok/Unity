# Этап 03_UI

## Расширение задач (Lineage II + Diablo III UI)

**Сначала прочитать:**
- `specs/FEATURE_MATRIX_L2_D3.md`
- `specs/UI_WINDOWS_CATALOG_L2_D3.md`
- `specs/UI_INTERACTIONS_L2_D3.md`
- `specs/LOOT_SYSTEM_D3.md`

### Обязательные deliverables этапа
1. Window Framework (uGUI): модальность, фокус, z-order, drag-move, dock/snap, сохранение layout.
2. HUD: player frame, target frame, action bar, belt, minimap, quest tracker, chat.
3. Окна: Inventory/Character/Skills/Map/Quest Journal + соц/торговля (как заглушки минимум).
4. Loot UI: world labels, ALT toggle, подсветка, фильтр, toast уведомления.
5. Прототип-ассеты: world-actors — Sphere/Cube; UI — TMP текст.

### Acceptance (минимум)
- Все окна из каталога существуют и управляются единым фреймворком.
- Инвентарь/экипировка: drag&drop и сравнение статов (текстовые статы).
- Лут: label на земле + ALT toggle + фильтр по редкости.
