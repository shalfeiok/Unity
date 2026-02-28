using Game.Infrastructure.Persistence;

namespace Game.Presentation.UI.Windowing
{
    public sealed class UILayoutPersistence
    {
        private readonly UILayoutRepository _repository;

        public UILayoutPersistence(UILayoutRepository repository)
        {
            _repository = repository;
        }

        public void Save(UILayoutState state) => _repository.Save(state);

        public UILayoutState LoadOrDefault()
        {
            return _repository.TryLoad(out var state) ? state : new UILayoutState();
        }
    }
}
