using UnityEngine;

namespace Game.Infrastructure.Config
{
    [CreateAssetMenu(menuName = "Game/Config/Perf Budget Settings", fileName = "PerfBudgetSettings")]
    public sealed class PerfBudgetSettings : ScriptableObject
    {
        [Min(1)]
        [SerializeField] private int uiRefreshActionsPerFrame = 32;

        [Min(0)]
        [SerializeField] private int pooledUiElementsWarmup = 64;

        [Min(1)]
        [SerializeField] private int maxEnemies = 150;

        [Min(1)]
        [SerializeField] private int maxProjectiles = 300;

        [Min(1)]
        [SerializeField] private int maxLootLabels = 120;

        public int UiRefreshActionsPerFrame => uiRefreshActionsPerFrame;
        public int PooledUiElementsWarmup => pooledUiElementsWarmup;
        public int MaxEnemies => maxEnemies;
        public int MaxProjectiles => maxProjectiles;
        public int MaxLootLabels => maxLootLabels;
    }
}
