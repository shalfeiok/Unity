using System.Collections.Generic;
using Game.Application.Events;
using Game.Application.Transactions;
using Game.Domain.Poe.Passives;

namespace Game.Application.Poe.UseCases
{
    public sealed class AllocatePassiveNodeUseCase
    {
        private readonly TransactionRunner _runner;
        private readonly PassiveTreeService _service;
        private readonly IApplicationEventPublisher _eventPublisher;

        public AllocatePassiveNodeUseCase(TransactionRunner runner, PassiveTreeService service, IApplicationEventPublisher eventPublisher = null)
        {
            _runner = runner;
            _service = service;
            _eventPublisher = eventPublisher;
        }

        public bool Execute(string operationId, PassiveTreeDefinition tree, HashSet<string> allocated, string nodeId, int points)
        {
            bool result = false;
            _runner.Run(
                operationId,
                validate: () => tree != null && allocated != null,
                apply: () => result = _service.TryAllocateNode(tree, allocated, nodeId, points),
                publish: () =>
                {
                    if (!result || _eventPublisher == null)
                        return;

                    _eventPublisher.Publish(new ApplicationEvent(
                        ApplicationEventType.PassiveAllocated,
                        operationId,
                        new Dictionary<string, string> { ["nodeId"] = nodeId }));
                });
            return result;
        }
    }
}
