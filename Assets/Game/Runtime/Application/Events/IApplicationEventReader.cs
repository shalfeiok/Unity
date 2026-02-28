using System.Collections.Generic;

namespace Game.Application.Events
{
    public interface IApplicationEventReader
    {
        IReadOnlyList<ApplicationEvent> Events { get; }
    }
}

