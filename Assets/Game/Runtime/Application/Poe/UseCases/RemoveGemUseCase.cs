using System.Collections.Generic;
using Game.Application.Events;
using Game.Application.Transactions;
using Game.Domain.Poe.Sockets;

namespace Game.Application.Poe.UseCases
{
    public sealed class RemoveGemUseCase
    {
        private readonly TransactionRunner _runner;
        private readonly SocketService _socketService;
        private readonly IApplicationEventPublisher _eventPublisher;

        public RemoveGemUseCase(TransactionRunner runner, SocketService socketService, IApplicationEventPublisher eventPublisher = null)
        {
            _runner = runner;
            _socketService = socketService;
            _eventPublisher = eventPublisher;
        }

        public bool Execute(string operationId, SocketModel sockets, int socketIndex, out string removedGemId)
        {
            removedGemId = string.Empty;
            bool removed = false;
            _runner.Run(
                operationId,
                validate: () => sockets != null,
                apply: () => removed = _socketService.TryRemoveGem(sockets, socketIndex, out removedGemId),
                publish: () =>
                {
                    if (!removed || _eventPublisher == null)
                        return;

                    _eventPublisher.Publish(new ApplicationEvent(
                        ApplicationEventType.GemRemoved,
                        operationId,
                        new Dictionary<string, string>
                        {
                            ["gemId"] = removedGemId,
                            ["socketIndex"] = socketIndex.ToString()
                        }));
                });

            return removed;
        }
    }
}
