using System.Collections.Generic;

namespace Game.Application.Events
{
    public sealed class InMemoryApplicationEventPublisher : IApplicationEventPublisher, IApplicationEventReader
    {
        private readonly List<ApplicationEvent> _events = new();

        public IReadOnlyList<ApplicationEvent> Events => _events;

        public void Publish(ApplicationEvent applicationEvent)
        {
            _events.Add(applicationEvent);
        }
    }
}
