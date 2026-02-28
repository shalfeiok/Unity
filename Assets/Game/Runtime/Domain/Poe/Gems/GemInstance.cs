namespace Game.Domain.Poe.Gems
{
    public readonly struct GemInstance
    {
        public GemInstance(string gemId, bool isSupport, int socketIndex)
        {
            GemId = gemId;
            IsSupport = isSupport;
            SocketIndex = socketIndex;
        }

        public string GemId { get; }
        public bool IsSupport { get; }
        public int SocketIndex { get; }
    }
}
