using System.IO;
using Game.Infrastructure.Persistence;
using Game.Presentation.UI.Windowing;
using NUnit.Framework;

namespace Game.Tests.EditMode.UI
{
    public sealed class UILayoutAutoSaveHooksTests
    {
        [Test]
        public void Hooks_SaveLayout_WhenKnownWindowChanges()
        {
            string path = Path.Combine(Path.GetTempPath(), "unity-ui-layout-tests", "autosave-layout.json");
            if (File.Exists(path)) File.Delete(path);

            var persistence = new UILayoutPersistence(new UILayoutRepository(new JsonStorage(), path));
            var state = new UILayoutState();
            state.windows.Add(new UILayoutState.WindowState { id = WindowId.Inventory, x = 1, y = 2, width = 3, height = 4 });
            var hooks = new UILayoutAutoSaveHooks(persistence, state);

            hooks.OnWindowDragEnded(WindowId.Inventory);

            Assert.True(File.Exists(path));
        }

        [Test]
        public void Hooks_DoNotSave_WhenWindowNotInState()
        {
            string path = Path.Combine(Path.GetTempPath(), "unity-ui-layout-tests", "autosave-layout-miss.json");
            if (File.Exists(path)) File.Delete(path);

            var persistence = new UILayoutPersistence(new UILayoutRepository(new JsonStorage(), path));
            var state = new UILayoutState();
            state.windows.Add(new UILayoutState.WindowState { id = WindowId.Inventory });
            var hooks = new UILayoutAutoSaveHooks(persistence, state);

            hooks.OnWindowResizeEnded(WindowId.Craft);

            Assert.False(File.Exists(path));
        }
    }
}
