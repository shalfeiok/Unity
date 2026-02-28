using Game.Presentation.UI.Windowing;
using UnityEngine;

namespace Game.Infrastructure.Persistence
{
    public sealed class UILayoutRepository
    {
        private readonly JsonStorage _storage;
        private readonly string _path;

        public UILayoutRepository(JsonStorage storage, string path)
        {
            _storage = storage;
            _path = path;
        }

        public void Save(UILayoutState state)
        {
            string json = JsonUtility.ToJson(state, true);
            _storage.Save(_path, json);
        }

        public bool TryLoad(out UILayoutState state)
        {
            if (!_storage.TryLoad(_path, out var json))
            {
                state = new UILayoutState();
                return false;
            }

            state = JsonUtility.FromJson<UILayoutState>(json) ?? new UILayoutState();
            return true;
        }
    }
}
