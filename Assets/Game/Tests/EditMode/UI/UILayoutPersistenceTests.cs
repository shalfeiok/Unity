using System.IO;
using Game.Infrastructure.Persistence;
using Game.Presentation.UI.Windowing;
using NUnit.Framework;

namespace Game.Tests.EditMode.UI
{
    public sealed class UILayoutPersistenceTests
    {
        [Test]
        public void SaveLoad_RoundTripsWindowLayoutJson()
        {
            string path = Path.Combine(Path.GetTempPath(), "unity-ui-layout-tests", "layout.json");
            if (File.Exists(path)) File.Delete(path);

            var storage = new JsonStorage();
            var repository = new UILayoutRepository(storage, path);
            var persistence = new UILayoutPersistence(repository);

            var state = new UILayoutState();
            state.windows.Add(new UILayoutState.WindowState
            {
                id = WindowId.Inventory,
                x = 12,
                y = 24,
                width = 640,
                height = 480,
                dock = "Left",
                zIndex = 10,
                active = true,
                customJson = "{\"k\":1}"
            });

            persistence.Save(state);
            var loaded = persistence.LoadOrDefault();

            Assert.AreEqual(1, loaded.windows.Count);
            Assert.AreEqual(WindowId.Inventory, loaded.windows[0].id);
            Assert.AreEqual(640, loaded.windows[0].width);
            Assert.AreEqual("Left", loaded.windows[0].dock);
        }
    }
}
