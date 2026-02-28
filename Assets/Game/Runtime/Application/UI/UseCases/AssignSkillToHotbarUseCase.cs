using Game.Application.Transactions;
using Game.Presentation.UI.Hud;

namespace Game.Application.UI.UseCases
{
    public sealed class AssignSkillToHotbarUseCase
    {
        private readonly TransactionRunner _runner;
        private readonly HotbarAssignmentService _hotbar;

        public AssignSkillToHotbarUseCase(TransactionRunner runner, HotbarAssignmentService hotbar)
        {
            _runner = runner;
            _hotbar = hotbar;
        }

        public bool Execute(string operationId, int slotIndex, string skillId)
        {
            bool ok = false;
            _runner.Run(
                operationId,
                validate: () => slotIndex >= 0 && slotIndex <= 9,
                apply: () => ok = _hotbar.Assign(slotIndex, skillId),
                publish: null);

            return ok;
        }

        public bool Unassign(string operationId, int slotIndex)
        {
            bool ok = false;
            _runner.Run(
                operationId,
                validate: () => slotIndex >= 0 && slotIndex <= 9,
                apply: () => ok = _hotbar.Unassign(slotIndex),
                publish: null);

            return ok;
        }
    }
}
