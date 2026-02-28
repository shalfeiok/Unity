using UnityEngine;

namespace Game.Infrastructure.Config
{
    public static class PerfBudgetProvider
    {
        public const string ResourcesPath = "Config/PerfBudgetSettings";

        public static PerfBudgetSettings LoadOrDefault()
        {
            var settings = Resources.Load<PerfBudgetSettings>(ResourcesPath);
            if (settings != null)
                return settings;

            var fallback = ScriptableObject.CreateInstance<PerfBudgetSettings>();
            return fallback;
        }
    }
}
