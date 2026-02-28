using Game.Domain.Poe.Flasks;

namespace Game.Presentation.UI.Hud.Flasks
{
    public sealed class FlaskBeltHudService
    {
        private readonly FlaskService _flaskService;

        public FlaskBeltHudService(FlaskService flaskService)
        {
            _flaskService = flaskService;
        }

        public void RefreshSlot(FlaskSlotState slot, FlaskDefinition definition)
        {
            slot.FlaskId = definition.Id;
            slot.MaxCharges = definition.MaxCharges;
            slot.Charges = _flaskService.GetCharges(definition.Id);
            slot.IsUsable = slot.Charges >= definition.ChargesPerUse;
        }
    }
}
