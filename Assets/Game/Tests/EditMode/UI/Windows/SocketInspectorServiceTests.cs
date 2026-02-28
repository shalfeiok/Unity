using System.Collections.Generic;
using Game.Domain.Poe.Gems;
using Game.Domain.Poe.Sockets;
using Game.Presentation.UI.Windowing;
using Game.Presentation.UI.Windows.Inventory;
using Game.Presentation.UI.Windows.Inventory.SocketInspector;
using NUnit.Framework;

namespace Game.Tests.EditMode.UI.Windows
{
    public sealed class SocketInspectorServiceTests
    {
        [Test]
        public void InsertDraggedGem_InsertsGemAndClearsPayload()
        {
            var dragDrop = new DragDropService();
            dragDrop.BeginDrag(new DragPayload(DragPayloadKind.GemRef, "gem_fireball"));

            var service = new SocketInspectorService(new SocketService(), new SkillBuildCompiler(), dragDrop);
            var sockets = new SocketModel(2);

            bool inserted = service.InsertDraggedGem(sockets, 0);

            Assert.True(inserted);
            Assert.AreEqual("gem_fireball", sockets.GemIds[0]);
            Assert.False(dragDrop.HasActivePayload);
        }

        [Test]
        public void RemoveGemToInventory_AddsGemToInventory()
        {
            var sockets = new SocketModel(2);
            sockets.GemIds[1] = "support_chain";
            var inventory = new InventoryWindowState();

            var service = new SocketInspectorService(new SocketService(), new SkillBuildCompiler(), new DragDropService());
            bool removed = service.RemoveGemToInventory(sockets, 1, inventory);

            Assert.True(removed);
            CollectionAssert.Contains(inventory.GemIds, "support_chain");
        }

        [Test]
        public void BuildViewModel_ContainsAppliedAndRejectedSupports()
        {
            var sockets = new SocketModel(3);
            sockets.GemIds[0] = "skill_fireball";
            sockets.GemIds[1] = "support_added";
            sockets.GemIds[2] = "support_bad";
            sockets.Links.Add(new LinkGroup { Indices = { 0, 1, 2 } });

            var skills = new Dictionary<string, SkillGemDefinition>
            {
                ["skill_fireball"] = new() { Id = "skill_fireball", SkillId = "Fireball", Tags = new[] { "spell" } }
            };
            var supports = new Dictionary<string, SupportGemDefinition>
            {
                ["support_added"] = new() { Id = "support_added", RequiredTags = new[] { "spell" } },
                ["support_bad"] = new() { Id = "support_bad", RequiredTags = new[] { "melee" } }
            };

            var vm = new SocketInspectorService(new SocketService(), new SkillBuildCompiler(), new DragDropService())
                .BuildViewModel(sockets, skills, supports);

            Assert.AreEqual("Fireball", vm.ActiveSkillId);
            CollectionAssert.Contains(vm.AppliedSupports, "support_added");
            Assert.AreEqual(1, vm.RejectedSupports.Count);
        }
    }
}
