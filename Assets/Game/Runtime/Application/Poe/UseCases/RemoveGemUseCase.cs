using Game.Application.Transactions;
using Game.Domain.Poe.Sockets;

namespace Game.Application.Poe.UseCases
{
    public sealed class RemoveGemUseCase
    {
        private readonly TransactionRunner _runner;
        private readonly SocketService _socketService;

        public RemoveGemUseCase(TransactionRunner runner, SocketService socketService)
        {
            _runner = runner;
            _socketService = socketService;
        }

        public bool Execute(string operationId, SocketModel sockets, int socketIndex, out string removedGemId)
        {
            removedGemId = string.Empty;
            bool removed = false;
            _runner.Run(
                operationId,
                validate: () => sockets != null,
                apply: () => removed = _socketService.TryRemoveGem(sockets, socketIndex, out removedGemId),
                publish: null);

            return removed;
        }
    }
}
