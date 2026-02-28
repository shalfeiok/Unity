namespace Game.Domain.Poe.Sockets
{
    public sealed class SocketService
    {
        public bool TryInsertGem(SocketModel model, int socketIndex, string gemId)
        {
            if (model == null || string.IsNullOrWhiteSpace(gemId))
                return false;

            if (socketIndex < 0 || socketIndex >= model.GemIds.Length)
                return false;

            if (!string.IsNullOrEmpty(model.GemIds[socketIndex]))
                return false;

            model.GemIds[socketIndex] = gemId;
            return true;
        }

        public bool TryRemoveGem(SocketModel model, int socketIndex, out string removedGemId)
        {
            removedGemId = string.Empty;
            if (model == null)
                return false;

            if (socketIndex < 0 || socketIndex >= model.GemIds.Length)
                return false;

            var existing = model.GemIds[socketIndex];
            if (string.IsNullOrEmpty(existing))
                return false;

            removedGemId = existing;
            model.GemIds[socketIndex] = string.Empty;
            return true;
        }
    }
}
