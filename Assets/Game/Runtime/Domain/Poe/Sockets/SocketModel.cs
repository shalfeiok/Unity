using System.Collections.Generic;

namespace Game.Domain.Poe.Sockets
{
    public sealed class SocketModel
    {
        public SocketModel(int count)
        {
            GemIds = new string[count];
        }

        public string[] GemIds { get; }
        public List<LinkGroup> Links { get; } = new();
    }
}
