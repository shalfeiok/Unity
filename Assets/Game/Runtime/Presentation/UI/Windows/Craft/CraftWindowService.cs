using System.Collections.Generic;
using Game.Application.Poe.UseCases;
using Game.Domain.Poe.Crafting;
using Game.Domain.Poe.Items;
using Game.Presentation.UI.Feedback;
using Game.Presentation.UI.Localization;

namespace Game.Presentation.UI.Windows.Craft
{
    public sealed class CraftWindowService
    {
        private readonly ApplyCurrencyActionUseCase _useCase;
        private readonly ILocalizationService _loc;
        private readonly UiEventLogService _eventLog;

        public CraftWindowService(
            ApplyCurrencyActionUseCase useCase,
            ILocalizationService localizationService = null,
            UiEventLogService eventLog = null)
        {
            _useCase = useCase;
            _loc = localizationService;
            _eventLog = eventLog;
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
            if (!ok)
            {
                state.LastErrorMessage = Translate("ui.error.precondition");
                state.LastResultMessage = string.Empty;
                _eventLog?.AddError(state.LastErrorMessage);
                return false;
            }

            state.CurrentModIds.Clear();
            for (int i = 0; i < updated.Mods.Count; i++)
                state.CurrentModIds.Add(updated.Mods[i].Definition.Id);

            state.HasPreview = false;
            state.PreviewModIds.Clear();
            state.LastErrorMessage = string.Empty;
            state.LastResultMessage = TranslateFormat("craft.apply.success", action.Id);
            _eventLog?.AddInfo(state.LastResultMessage);
            return true;
        }

        private string Translate(string key)
        {
            return _loc == null ? key : _loc.Translate(key);
        }

        private string TranslateFormat(string key, params object[] args)
        {
            return _loc == null ? key : _loc.TranslateFormat(key, args);
        }
    }
}
