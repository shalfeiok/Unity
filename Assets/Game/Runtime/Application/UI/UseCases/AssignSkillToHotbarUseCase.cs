using System.Collections.Generic;
using Game.Application.Events;
using Game.Application.Transactions;
using Game.Presentation.UI.Hud;

namespace Game.Application.UI.UseCases
{
    public sealed class AssignSkillToHotbarUseCase
    {
        private readonly TransactionRunner _runner;
        private readonly HotbarAssignmentService _hotbar;
        private readonly IApplicationEventPublisher _eventPublisher;

        public AssignSkillToHotbarUseCase(TransactionRunner runner, HotbarAssignmentService hotbar, IApplicationEventPublisher eventPublisher = null)
        {
            _runner = runner;
            _hotbar = hotbar;
            _eventPublisher = eventPublisher;
        }

        public bool Execute(string operationId, int slotIndex, string skillId)
        {
            bool ok = false;
            _runner.Run(
                operationId,
                validate: () => slotIndex >= 0 && slotIndex <= 9,
                apply: () => ok = _hotbar.Assign(slotIndex, skillId),
                publish: () =>
                {
                    if (!ok || _eventPublisher == null)
                        return;

                    _eventPublisher.Publish(new ApplicationEvent(
                        ApplicationEventType.HotbarAssigned,
                        operationId,
                        new Dictionary<string, string>
                        {
                            ["slot"] = slotIndex.ToString(),
                            ["skillId"] = skillId ?? string.Empty
                        }));
                });

            return ok;
        }

        public bool Unassign(string operationId, int slotIndex)
        {
            bool ok = false;
            _runner.Run(
                operationId,
                validate: () => slotIndex >= 0 && slotIndex <= 9,
                apply: () => ok = _hotbar.Unassign(slotIndex),
                publish: () =>
                {
                    if (!ok || _eventPublisher == null)
                        return;

                    _eventPublisher.Publish(new ApplicationEvent(
                        ApplicationEventType.HotbarUnassigned,
                        operationId,
                        new Dictionary<string, string>
                        {
                            ["slot"] = slotIndex.ToString()
                        }));
                });

            return ok;
        }
    }
}
