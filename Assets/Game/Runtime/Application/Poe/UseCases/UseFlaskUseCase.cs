using System.Collections.Generic;
using Game.Application.Events;
using Game.Application.Transactions;
using Game.Domain.Poe.Flasks;

namespace Game.Application.Poe.UseCases
{
    public sealed class UseFlaskUseCase
    {
        private readonly TransactionRunner _runner;
        private readonly FlaskService _service;
        private readonly IApplicationEventPublisher _eventPublisher;

        public UseFlaskUseCase(TransactionRunner runner, FlaskService service, IApplicationEventPublisher eventPublisher = null)
        {
            _runner = runner;
            _service = service;
            _eventPublisher = eventPublisher;
        }

        public bool Execute(string operationId, FlaskDefinition flask)
        {
            bool used = false;
            _runner.Run(
                operationId,
                validate: () => flask != null,
                apply: () => used = _service.TryUse(flask),
                publish: () =>
                {
                    if (!used || _eventPublisher == null)
                        return;

                    _eventPublisher.Publish(new ApplicationEvent(
                        ApplicationEventType.FlaskUsed,
                        operationId,
                        new Dictionary<string, string> { ["flaskId"] = flask.Id }));
                });

            return used;
        }
    }
}
