using System.Collections.Generic;
using Game.Domain.Poe.Items;

namespace Game.Infrastructure.DataPipeline
{
    public sealed class ModPoolValidator
    {
        public bool Validate(IReadOnlyList<ModDefinition> mods, out string error)
        {
            var seen = new HashSet<string>();
            for (int i = 0; i < mods.Count; i++)
            {
                var mod = mods[i];
                if (string.IsNullOrWhiteSpace(mod.Id))
                {
                    error = "Mod id is empty.";
                    return false;
                }

                if (!seen.Add(mod.Id))
                {
                    error = $"Duplicate mod id: {mod.Id}";
                    return false;
                }

                if (mod.MinItemLevel < 1)
                {
                    error = $"Invalid ilvl gate for mod: {mod.Id}";
                    return false;
                }

                if (mod.MinValue > mod.MaxValue)
                {
                    error = $"Invalid roll range for mod: {mod.Id}";
                    return false;
                }
            }

            error = string.Empty;
            return true;
        }
    }
}
