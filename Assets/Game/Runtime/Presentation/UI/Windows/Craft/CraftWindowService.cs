using System.Collections.Generic;
using Game.Application.Poe.UseCases;
using Game.Domain.Poe.Crafting;
using Game.Domain.Poe.Items;

namespace Game.Presentation.UI.Windows.Craft
{
    public sealed class CraftWindowService
    {
        private readonly ApplyCurrencyActionUseCase _useCase;

        public CraftWindowService(ApplyCurrencyActionUseCase useCase)
        {
            _useCase = useCase;
        }

        public void SelectAction(CraftWindowState state, string actionId)
        {
            state.SelectedCurrencyActionId = actionId ?? string.Empty;
        }

        public void BuildPreview(CraftWindowState state, GeneratedPoeItem item)
        {
            state.PreviewModIds.Clear();
            for (int i = 0; i < item.Mods.Count; i++)
                state.PreviewModIds.Add(item.Mods[i].Definition.Id);
            state.HasPreview = true;
        }

        public bool Apply(
            CraftWindowState state,
            string operationId,
            CurrencyActionDefinition action,
            GeneratedPoeItem item,
            IReadOnlyList<ModDefinition> pool,
            out GeneratedPoeItem updated)
        {
            var ok = _useCase.Execute(operationId, action, item, pool, out updated);
            if (!ok) return false;

            state.CurrentModIds.Clear();
            for (int i = 0; i < updated.Mods.Count; i++)
                state.CurrentModIds.Add(updated.Mods[i].Definition.Id);

            state.HasPreview = false;
            state.PreviewModIds.Clear();
            return true;
        }
    }
}
