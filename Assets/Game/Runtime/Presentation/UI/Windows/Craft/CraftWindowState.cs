using System.Collections.Generic;

namespace Game.Presentation.UI.Windows.Craft
{
    public sealed class CraftWindowState
    {
        public string ItemBaseId = string.Empty;
        public List<string> CurrentModIds = new();
        public string SelectedCurrencyActionId = string.Empty;
        public List<string> PreviewModIds = new();
        public bool HasPreview;
    }
}
