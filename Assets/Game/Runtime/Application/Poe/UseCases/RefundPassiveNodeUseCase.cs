using System.Collections.Generic;
using Game.Application.Transactions;
using Game.Domain.Poe.Passives;

namespace Game.Application.Poe.UseCases
{
    public sealed class RefundPassiveNodeUseCase
    {
        private readonly TransactionRunner _runner;
        private readonly PassiveTreeService _service;

        public RefundPassiveNodeUseCase(TransactionRunner runner, PassiveTreeService service)
        {
            _runner = runner;
            _service = service;
        }

        public bool Execute(string operationId, PassiveTreeDefinition tree, HashSet<string> allocated, string nodeId)
        {
            bool result = false;
            _runner.Run(
                operationId,
                validate: () => tree != null && allocated != null,
                apply: () => result = _service.TryRefundNode(tree, allocated, nodeId),
                publish: null);

            return result;
        }
    }
}
