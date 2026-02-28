# Этап MOV — Список задач (конкретно)

- MOV-01 — InputRouter + WorldRaycaster + блок UI ввода.
- MOV-02 — Click-to-move: NavMeshMoveController + PlayerCommandController.MoveTo.
- MOV-03 — OrbitCameraController: RMB orbit, pitch clamp, zoom smoothing.
- MOV-04 — Camera collision: spherecast, anti-jitter, min distance.
- MOV-05 — Targeting: ITargetable + selection highlight + Target UI feed.
- MOV-06 — Cast queue (1 слот) + политики replace/drop.
- MOV-07 — Auto-approach: если target вне range → approach then cast.
- MOV-08 — Stop-on-cast + (опц) GCD, настройки.
