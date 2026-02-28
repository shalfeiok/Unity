using System.Collections.Generic;
using Game.Presentation.UI.Tooltips;

namespace Game.Presentation.UI.Windows.Skills
{
    public sealed class SkillsPanelEntry
    {
        public string SkillId;
        public List<string> AppliedSupports = new();
        public List<string> RejectedSupportReasons = new();
        public TooltipModel ExplainTooltip;
    }
}
