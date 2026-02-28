# CHANGELOG

## 2026-02-28
- Added modal lifecycle hooks in `UIInputRouter` (`OnModalOpened`/`OnModalClosed`) and aligned back-navigation contract with `EnterModal`/`ExitModal`.
- Made hotkey toggling state-aware via `UIHotkeyRouter.TryToggle(..., out windowId, out isOpen)` and wired deterministic back-navigation notifications in `UIInputRouter`.
- Added `WindowStackBackNavigation` and integrated window-state notifications from `UIInputRouter` for close-top-panel behavior.
- Added ESC back-navigation contract (`IUIBackNavigation`) and `UIInputRouter.TryHandleEscape()` priority (close modal before top panel).
- Added `UIInputRouter` + `DefaultUIHotkeyResolver` to route raw key intents into hotkey toggles and respect modal input context.
- Added PlayMode smoke coverage `UITogglesSmokeTest` for hotkey-intent panel toggles (I/C/P/S/K/O/M) via `UIHotkeyRouter` + windowing runtime, including Atlas mapping and injectable bindings (`IUIHotkeyBindings` / `DefaultUIHotkeyBindings`).
- Added PlayMode smoke scenario that stitches hotbar assignment, passive allocation, and craft preview/commit into one service-level vertical loop.
- Added top-level project reports: `SUMMARY_IMPLEMENTATION_REPORT.md`, `ARCHITECTURE_OVERVIEW.md`, `SYSTEM_DEPENDENCY_GRAPH.md`, `BALANCE_GUIDELINES.md`.
