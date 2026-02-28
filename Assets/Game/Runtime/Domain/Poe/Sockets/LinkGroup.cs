using System.Collections.Generic;

namespace Game.Domain.Poe.Sockets
{
    public sealed class LinkGroup
    {
        public HashSet<int> Indices { get; } = new();

        public bool Contains(int socketIndex) => Indices.Contains(socketIndex);
    }
}
