using System.Collections.Generic;
using Game.Presentation.UI.Windowing;
using NUnit.Framework;

namespace Game.Tests.EditMode.UI
{
    public sealed class InputAndDragDropTests
    {
        [Test]
        public void InputContextStack_PushPop_ReturnsToGameplay()
        {
            var stack = new InputContextStack();
            stack.Push(InputContext.UI);
            stack.Push(InputContext.Modal);

            Assert.AreEqual(InputContext.Modal, stack.Current);
            stack.Pop();
            stack.Pop();
            stack.Pop();

            Assert.AreEqual(InputContext.Gameplay, stack.Current);
        }

        [Test]
        public void DragDropService_BeginEndDrag_RoundTripsPayload()
        {
            var service = new DragDropService();
            service.BeginDrag(new DragPayload(DragPayloadKind.GemRef, "gem_1"));

            Assert.True(service.HasActivePayload);
            var payload = service.EndDrag();

            Assert.AreEqual(DragPayloadKind.GemRef, payload.Kind);
            Assert.AreEqual("gem_1", payload.Id);
            Assert.False(service.HasActivePayload);
        }

        [Test]
        public void VirtualizedListView_ReturnsOnlyVisibleSlice()
        {
            var source = new List<int> { 1, 2, 3, 4, 5, 6 };
            var view = new VirtualizedListView<int>(source);

            var visible = view.GetVisibleRange(2, 3);

            CollectionAssert.AreEqual(new[] { 3, 4, 5 }, visible);
        }
    }
}
