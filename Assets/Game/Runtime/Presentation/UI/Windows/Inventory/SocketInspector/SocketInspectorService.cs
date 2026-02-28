using System.Collections.Generic;
using Game.Domain.Poe.Gems;
using Game.Domain.Poe.Sockets;
using Game.Presentation.UI.Windowing;

namespace Game.Presentation.UI.Windows.Inventory.SocketInspector
{
    public sealed class SocketInspectorService
    {
        private readonly SocketService _socketService;
        private readonly SkillBuildCompiler _compiler;
        private readonly DragDropService _dragDrop;

        public SocketInspectorService(SocketService socketService, SkillBuildCompiler compiler, DragDropService dragDrop)
        {
            _socketService = socketService;
            _compiler = compiler;
            _dragDrop = dragDrop;
        }

        public bool InsertDraggedGem(SocketModel sockets, int socketIndex)
        {
            if (!_dragDrop.HasActivePayload) return false;
            var payload = _dragDrop.Current;
            if (payload.Kind != DragPayloadKind.GemRef) return false;

            bool ok = _socketService.TryInsertGem(sockets, socketIndex, payload.Id);
            if (ok)
                _dragDrop.EndDrag();

            return ok;
        }

        public bool RemoveGemToInventory(SocketModel sockets, int socketIndex, Inventory.InventoryWindowState inventory)
        {
            if (!_socketService.TryRemoveGem(sockets, socketIndex, out var gemId))
                return false;

            inventory.AddGem(gemId);
            return true;
        }

        public SocketInspectorViewModel BuildViewModel(
            SocketModel sockets,
            IReadOnlyDictionary<string, SkillGemDefinition> skills,
            IReadOnlyDictionary<string, SupportGemDefinition> supports)
        {
            var compiled = _compiler.Build(sockets, skills, supports);
            return new SocketInspectorViewModel
            {
                ActiveSkillId = compiled.SkillId,
                AppliedSupports = new List<string>(compiled.AppliedSupportGemIds),
                RejectedSupports = new List<RejectedSupport>(compiled.RejectedSupports)
            };
        }
    }
}
