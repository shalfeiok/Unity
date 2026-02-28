using System.Collections.Generic;

namespace Game.Presentation.UI.Tooltips
{
    public sealed class TooltipModel
    {
        public string Title;
        public string Subtitle;
        public readonly List<TooltipSection> Sections = new();
    }

    public sealed class TooltipSection
    {
        public string Header;
        public readonly List<string> Lines = new();
    }
}
