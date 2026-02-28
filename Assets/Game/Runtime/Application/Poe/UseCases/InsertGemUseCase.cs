using Game.Application.Transactions;
using Game.Domain.Poe.Sockets;

namespace Game.Application.Poe.UseCases
{
    public sealed class InsertGemUseCase
    {
        private readonly TransactionRunner _runner;
        private readonly SocketService _socketService;

        public InsertGemUseCase(TransactionRunner runner, SocketService socketService)
        {
            _runner = runner;
            _socketService = socketService;
        }

        public bool Execute(string operationId, SocketModel sockets, int socketIndex, string gemId)
        {
            bool inserted = false;
            _runner.Run(
                operationId,
                validate: () => sockets != null && !string.IsNullOrWhiteSpace(gemId),
                apply: () => inserted = _socketService.TryInsertGem(sockets, socketIndex, gemId),
                publish: null);

            return inserted;
        }
    }
}
