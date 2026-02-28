using System.Collections.Generic;
using Game.Application.Transactions;
using Game.Domain.Poe.Passives;

namespace Game.Application.Poe.UseCases
{
    public sealed class AllocatePassiveNodeUseCase
    {
        private readonly TransactionRunner _runner;
        private readonly PassiveTreeService _service;

        public AllocatePassiveNodeUseCase(TransactionRunner runner, PassiveTreeService service)
        {
            _runner = runner;
            _service = service;
        }

        public bool Execute(string operationId, PassiveTreeDefinition tree, HashSet<string> allocated, string nodeId, int points)
        {
            bool result = false;
            _runner.Run(
                operationId,
                validate: () => tree != null && allocated != null,
                apply: () => result = _service.TryAllocateNode(tree, allocated, nodeId, points),
                publish: null);
            return result;
        }
    }
}
