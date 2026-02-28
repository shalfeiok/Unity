using System.Collections.Generic;
using Game.Application.Poe.UseCases;
using Game.Application.Transactions;
using Game.Application.UI.UseCases;
using Game.Domain.Poe.Crafting;
using Game.Domain.Poe.Items;
using Game.Domain.Poe.Passives;
using Game.Domain.Rng;
using Game.Infrastructure.Economy;
using Game.Presentation.UI.Hud;
using Game.Presentation.UI.Windowing;
using Game.Presentation.UI.Windows.Craft;
using NUnit.Framework;

namespace Game.Tests.PlayMode.UI
{
    public sealed class UITogglesSmokeTest
    {
        [Test]
        public void ToggleCorePanels_ByHotkeyIntent_Smoke()
        {
            var registry = new WindowRegistry();
            var service = new WindowService(registry);
            var manager = new WindowManager(service);

            var hotkeyMapping = new Dictionary<string, WindowId>
            {
                ["I"] = WindowId.Inventory,
                ["C"] = WindowId.Character,
                ["P"] = WindowId.PassiveTree,
                ["S"] = WindowId.Skills,
                ["K"] = WindowId.Craft,
                ["O"] = WindowId.Craft,
                ["M"] = WindowId.PassiveTree
            };

            foreach (var mapping in hotkeyMapping)
            {
                bool visible = false;
                registry.Register(mapping.Value, state => visible = state);

                manager.Toggle(mapping.Value);
                Assert.True(service.IsOpen(mapping.Value), $"Expected {mapping.Key} to open {mapping.Value}.");
                Assert.True(visible, $"Expected visibility callback for {mapping.Value}.");

                manager.Toggle(mapping.Value);
                Assert.False(service.IsOpen(mapping.Value), $"Expected {mapping.Key} to close {mapping.Value}.");
            }
        }

        [Test]
        public void VerticalSliceCoreLoop_UseCasesAndUiServices_Smoke()
        {
            var hotbar = new HotbarAssignmentService();
            var assignUseCase = new AssignSkillToHotbarUseCase(new TransactionRunner(), hotbar);
            Assert.True(assignUseCase.Execute("pm_hotbar_assign", 1, "skill_fireball"));
            Assert.True(hotbar.TryGetAssignedSkill(1, out var equippedSkill));
            Assert.AreEqual("skill_fireball", equippedSkill);

            var passiveTree = new PassiveTreeDefinition();
            passiveTree.Nodes.Add(new PassiveNodeDefinition { Id = "start", IsStart = true, Neighbors = new List<string> { "n1" } });
            passiveTree.Nodes.Add(new PassiveNodeDefinition { Id = "n1", Neighbors = new List<string> { "start" } });

            var allocated = new HashSet<string>();
            var allocatePassive = new AllocatePassiveNodeUseCase(new TransactionRunner(), new PassiveTreeService());
            Assert.True(allocatePassive.Execute("pm_passive_start", passiveTree, allocated, "start", 2));
            Assert.True(allocatePassive.Execute("pm_passive_n1", passiveTree, allocated, "n1", 2));

            var craftUseCase = new ApplyCurrencyActionUseCase(
                new TransactionRunner(),
                new CurrencyActionEngine(new XorShift32Rng(777)),
                new TransactionLedger());
            var craftService = new CraftWindowService(craftUseCase);
            var craftState = new CraftWindowState();

            var itemBase = new ItemBaseDefinition { Id = "base_armor", ItemClass = "Armor", RequiredItemLevel = 1 };
            var item = new GeneratedPoeItem(itemBase, 20, new List<GeneratedPoeMod>());
            var pool = new List<ModDefinition>
            {
                new() { Id = "p_armor_flat", Group = "armor", IsPrefix = true, MinItemLevel = 1, MinValue = 5, MaxValue = 10 }
            };
            var action = new CurrencyActionDefinition { Id = "pm_add_prefix", Kind = CurrencyActionKind.AddRandomPrefix, Cost = 1 };

            craftService.BuildPreview(craftState, item);
            Assert.True(craftState.HasPreview);
            Assert.True(craftService.Apply(craftState, "pm_craft_apply", action, item, pool, out var updated));
            Assert.AreEqual(1, updated.Mods.Count);
        }
    }
}
