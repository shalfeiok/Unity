using System;
using System.IO;
using Game.Infrastructure.Persistence;
using Game.Presentation.UI.Windowing;
using NUnit.Framework;
using UnityEngine;

namespace Game.Tests.PlayMode.UI.Windowing
{
    public sealed class WindowingSmokeTests
    {
        [Test]
        public void OpenAndToggle_ManyWindows_Smoke()
        {
            var registry = new WindowRegistry();
            var service = new WindowService(registry);
            var manager = new WindowManager(service);

            int[] ids =
            {
                (int)WindowId.Inventory,
                (int)WindowId.Character,
                (int)WindowId.Skills,
                (int)WindowId.Craft,
                (int)WindowId.PassiveTree
            };

            for (int i = 0; i < 20; i++)
            {
                var id = (WindowId)ids[i % ids.Length];
                bool visible = false;
                registry.Register(id, v => visible = v);

                manager.Show(id);
                Assert.True(service.IsOpen(id));
                Assert.True(visible);

                manager.Toggle(id);
                Assert.False(service.IsOpen(id));
            }
        }

        [Test]
        public void DragMoveAndLayoutPersistence_SaveReload_Smoke()
        {
            string path = Path.Combine(Path.GetTempPath(), "unity-windowing-smoke", Guid.NewGuid() + ".json");

            var state = new UILayoutState();
            var pos = WindowDragMove.ApplyDrag(new Vector2(100, 200), new Vector2(50, -20), 1f);
            state.windows.Add(new UILayoutState.WindowState
            {
                id = WindowId.Inventory,
                x = pos.x,
                y = pos.y,
                width = 640,
                height = 480,
                zIndex = 7,
                active = true,
                dock = "None",
                customJson = "{\"smoke\":true}"
            });

            var repo = new UILayoutRepository(new JsonStorage(), path);
            var persistence = new UILayoutPersistence(repo);

            persistence.Save(state);
            var loaded = persistence.LoadOrDefault();

            Assert.AreEqual(1, loaded.windows.Count);
            Assert.AreEqual(WindowId.Inventory, loaded.windows[0].id);
            Assert.AreEqual(150f, loaded.windows[0].x);
            Assert.AreEqual(180f, loaded.windows[0].y);
        }
    }
}
