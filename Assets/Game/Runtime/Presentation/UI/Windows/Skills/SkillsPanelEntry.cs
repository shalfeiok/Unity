using System.Collections.Generic;

namespace Game.Presentation.UI.Windows.Skills
{
    public sealed class SkillsPanelEntry
    {
        public string SkillId;
        public List<string> AppliedSupports = new();
        public List<string> RejectedSupportReasons = new();
    }
}
