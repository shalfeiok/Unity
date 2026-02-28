using System.Collections.Generic;

namespace Game.Presentation.UI.Hud.Flasks
{
    public sealed class FlaskBeltHudState
    {
        public List<FlaskSlotState> Slots = new();
    }

    public sealed class FlaskSlotState
    {
        public string FlaskId = string.Empty;
        public int Charges;
        public int MaxCharges;
        public bool IsUsable;
    }
}
