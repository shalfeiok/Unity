# UI_WINDOWING_FRAMEWORK.md

Дата: 2026-02-27

Цель: стабильный, перемещаемый, лёгкий UI с множеством окон.

## Архитектура
- UIRoot (single canvas) + WindowManager (single owner)
- открытие через IWindowService
- события вместо Update, UI batching

## Window prefab contract
Chrome + Content + Overlay.
Компоненты: WindowRoot/TitleBar/Buttons/DragMove/Focus/Resize/DockSnap.

## Drag move (stable)
PointerDown offset (local), Drag → anchoredPosition (canvas scale aware),
Clamp to safe-area, EndDrag → snap/dock → save.

## Layout persistence
UILayoutState: pos/size/dock/collapsed/pinned/zIndex/activeTab/customJson.

## Input focus stack + DragDropService
Gameplay/UI/ChatTyping/Modal + universal drag&drop payloads.

## Performance
virtualized lists, pools for tooltips/toasts/labels, refresh scheduler.

Acceptance: 20+ окон без дерганий и без GC spikes.


## План реализации (атомарно)
- `specs/UI_IMPLEMENTATION_PLAN_WINDOWING.md`
