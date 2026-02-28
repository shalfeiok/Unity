using Game.Presentation.UI.Windowing;
using NUnit.Framework;
using UnityEngine;

namespace Game.Tests.EditMode.UI
{
    public sealed class WindowInteractionTests
    {
        [Test]
        public void DragMove_IsCanvasScaleAware()
        {
            var result = WindowDragMove.ApplyDrag(new Vector2(10f, 10f), new Vector2(20f, 10f), 2f);
            Assert.AreEqual(new Vector2(20f, 15f), result);
        }

        [Test]
        public void Resize_ClampsToMinMax()
        {
            var result = WindowResize.Resize(new Vector2(200f, 100f), new Vector2(-500f, 1000f), new Vector2(120f, 80f), new Vector2(300f, 220f));
            Assert.AreEqual(new Vector2(120f, 220f), result);
        }

        [Test]
        public void DockSnap_SnapsWhenCloseToBounds()
        {
            var result = WindowDockSnap.SnapToBounds(new Vector2(5f, 96f), new Rect(0f, 0f, 100f, 100f), 6f);
            Assert.AreEqual(new Vector2(0f, 100f), result);
        }

        [Test]
        public void Focus_AssignsIncreasingZ()
        {
            var focus = new WindowFocusController();
            int z1 = focus.Focus(WindowId.Inventory);
            int z2 = focus.Focus(WindowId.Character);

            Assert.Greater(z2, z1);
            Assert.AreEqual(z2, focus.GetZ(WindowId.Character));
        }

        [Test]
        public void ModalBlocker_TracksDepth()
        {
            var blocker = new WindowModalBlocker();
            blocker.PushModal();
            blocker.PushModal();
            blocker.PopModal();

            Assert.True(blocker.IsBlocking);
            blocker.PopModal();
            Assert.False(blocker.IsBlocking);
        }
    }
}
