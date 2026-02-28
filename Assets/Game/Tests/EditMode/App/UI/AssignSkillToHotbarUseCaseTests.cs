using Game.Application.Transactions;
using Game.Application.UI.UseCases;
using Game.Presentation.UI.Hud;
using NUnit.Framework;

namespace Game.Tests.EditMode.App.UI
{
    public sealed class AssignSkillToHotbarUseCaseTests
    {
        [Test]
        public void Execute_AssignsSkill_AndIsIdempotentByOperationId()
        {
            var runner = new TransactionRunner();
            var hotbar = new HotbarAssignmentService();
            var useCase = new AssignSkillToHotbarUseCase(runner, hotbar);

            Assert.True(useCase.Execute("op_hotbar_assign", 1, "Fireball"));
            Assert.True(useCase.Execute("op_hotbar_assign", 1, "IceNova")); // same op should not re-apply
            Assert.True(hotbar.TryGetAssignedSkill(1, out var skill));
            Assert.AreEqual("Fireball", skill);
        }

        [Test]
        public void Unassign_RemovesSkillFromSlot()
        {
            var runner = new TransactionRunner();
            var hotbar = new HotbarAssignmentService();
            var useCase = new AssignSkillToHotbarUseCase(runner, hotbar);

            Assert.True(useCase.Execute("op_hotbar_assign2", 2, "LeapSlam"));
            Assert.True(useCase.Unassign("op_hotbar_unassign2", 2));
            Assert.False(hotbar.TryGetAssignedSkill(2, out _));
        }
    }
}
