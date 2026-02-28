using Game.Infrastructure.Config;
using NUnit.Framework;
using UnityEngine;

namespace Game.Tests.EditMode
{
    public sealed class PerfBudgetEnforcerTests
    {
        [Test]
        public void Enforcer_RejectsEntityReservationsOverBudget()
        {
            var settings = ScriptableObject.CreateInstance<PerfBudgetSettings>();
            var enforcer = new PerfBudgetEnforcer(settings);

            Assert.True(enforcer.TryReserveEnemies(149, 1));
            Assert.False(enforcer.TryReserveEnemies(149, 2));

            Assert.True(enforcer.TryReserveProjectiles(299, 1));
            Assert.False(enforcer.TryReserveProjectiles(300, 1));

            Assert.True(enforcer.TryReserveLootLabels(119, 1));
            Assert.False(enforcer.TryReserveLootLabels(120, 1));
        }

        [Test]
        public void Enforcer_ClampsUiRefreshBudget()
        {
            var settings = ScriptableObject.CreateInstance<PerfBudgetSettings>();
            var enforcer = new PerfBudgetEnforcer(settings);

            Assert.AreEqual(0, enforcer.ClampUiRefreshBudget(-5));
            Assert.AreEqual(16, enforcer.ClampUiRefreshBudget(16));
            Assert.AreEqual(settings.UiRefreshActionsPerFrame, enforcer.ClampUiRefreshBudget(999));
        }
    }
}
