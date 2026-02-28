using System.Collections.Generic;
using Game.Domain.Poe.Gems;

namespace Game.Presentation.UI.Windows.Inventory.SocketInspector
{
    public sealed class SocketInspectorViewModel
    {
        public string ActiveSkillId;
        public List<string> AppliedSupports = new();
        public List<RejectedSupport> RejectedSupports = new();
    }
}
