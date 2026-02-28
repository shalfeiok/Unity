using System.Collections.Generic;
using Game.Application.Events;
using Game.Application.Transactions;
using Game.Domain.Poe.Sockets;

namespace Game.Application.Poe.UseCases
{
    public sealed class InsertGemUseCase
    {
        private readonly TransactionRunner _runner;
        private readonly SocketService _socketService;
        private readonly IApplicationEventPublisher _eventPublisher;

        public InsertGemUseCase(TransactionRunner runner, SocketService socketService, IApplicationEventPublisher eventPublisher = null)
        {
            _runner = runner;
            _socketService = socketService;
            _eventPublisher = eventPublisher;
        }

        public bool Execute(string operationId, SocketModel sockets, int socketIndex, string gemId)
        {
            bool inserted = false;
            _runner.Run(
                operationId,
                validate: () => sockets != null && !string.IsNullOrWhiteSpace(gemId),
                apply: () => inserted = _socketService.TryInsertGem(sockets, socketIndex, gemId),
                publish: () =>
                {
                    if (!inserted || _eventPublisher == null)
                        return;

                    _eventPublisher.Publish(new ApplicationEvent(
                        ApplicationEventType.GemInserted,
                        operationId,
                        new Dictionary<string, string>
                        {
                            ["gemId"] = gemId,
                            ["socketIndex"] = socketIndex.ToString()
                        }));
                });

            return inserted;
        }
    }
}
