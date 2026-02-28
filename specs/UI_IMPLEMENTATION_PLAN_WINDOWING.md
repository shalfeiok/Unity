# UI_IMPLEMENTATION_PLAN_WINDOWING.md

Дата: 2026-02-27

Цель: разложить Windowing UI framework на атомарные задачи/классы/файлы.

## 1) Файловая структура (Presentation/UI)
- Windowing/
  - WindowManager.cs
  - WindowRoot.cs
  - WindowRegistry.cs (id → prefab)
  - WindowService.cs (IWindowService)
  - WindowFocusController.cs
  - WindowModalBlocker.cs
  - WindowDragMove.cs
  - WindowResize.cs
  - WindowDockSnap.cs
  - UILayoutState.cs (DTO)
  - UILayoutPersistence.cs (Infrastructure adapter)
  - InputContextStack.cs
  - DragDropService.cs
  - UIRefreshScheduler.cs
  - VirtualizedListView.cs (+ pooling)

## 2) Контракты (публичные методы)
### IWindowService
- Open(WindowId id, object? args)
- Close(WindowId id)
- Toggle(WindowId id)
- BringToFront(WindowId id)
- GetState(WindowId id)

### WindowRoot
- WindowId
- SetContent(GameObject prefab)
- ApplyState(WindowState)
- CaptureState() -> WindowState

### WindowDragMove
- BeginDrag(pointer)
- Drag(pointer)
- EndDrag(pointer)

## 3) Acceptance “готово”
- 20 окон: открытие/закрытие, фокус, z-order
- перетаскивание корректно при canvas scale
- layout сохраняется/восстанавливается
- модалки блокируют клики под ними
- drag&drop работает через единый сервис
- списки инвентаря/чата виртуализированы
