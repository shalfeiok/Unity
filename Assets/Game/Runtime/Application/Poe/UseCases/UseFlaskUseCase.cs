using Game.Application.Transactions;
using Game.Domain.Poe.Flasks;

namespace Game.Application.Poe.UseCases
{
    public sealed class UseFlaskUseCase
    {
        private readonly TransactionRunner _runner;
        private readonly FlaskService _service;

        public UseFlaskUseCase(TransactionRunner runner, FlaskService service)
        {
            _runner = runner;
            _service = service;
        }

        public bool Execute(string operationId, FlaskDefinition flask)
        {
            return _runner.Run(
                operationId,
                validate: () => flask != null,
                apply: () => _service.TryUse(flask),
                publish: null);
        }
    }
}
