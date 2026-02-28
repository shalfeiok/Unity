namespace Game.Infrastructure.Config
{
    public sealed class PerfBudgetEnforcer
    {
        private readonly PerfBudgetSettings _settings;

        public PerfBudgetEnforcer(PerfBudgetSettings settings)
        {
            _settings = settings;
        }

        public bool TryReserveEnemies(int currentCount, int toAdd) => currentCount + toAdd <= _settings.MaxEnemies;
        public bool TryReserveProjectiles(int currentCount, int toAdd) => currentCount + toAdd <= _settings.MaxProjectiles;
        public bool TryReserveLootLabels(int currentCount, int toAdd) => currentCount + toAdd <= _settings.MaxLootLabels;

        public int ClampUiRefreshBudget(int requested)
        {
            if (requested < 0) return 0;
            if (requested > _settings.UiRefreshActionsPerFrame)
                return _settings.UiRefreshActionsPerFrame;
            return requested;
        }
    }
}
