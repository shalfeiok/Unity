using System.Collections;
using Game.Presentation;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Game.Tests.PlayMode
{
    public class WindowManagerSmokeTests
    {
        [UnityTest]
        public IEnumerator Register_FocusesLastWindow()
        {
            var root = new GameObject("Root", typeof(RectTransform));
            var managerObject = new GameObject("WindowManager");
            var manager = managerObject.AddComponent<WindowManager>();

            var windowA = new GameObject("A", typeof(RectTransform)).GetComponent<RectTransform>();
            windowA.SetParent(root.transform, false);
            var windowB = new GameObject("B", typeof(RectTransform)).GetComponent<RectTransform>();
            windowB.SetParent(root.transform, false);

            manager.Register(windowA);
            manager.Register(windowB);

            yield return null;
            Assert.AreEqual(root.transform.childCount - 1, windowB.GetSiblingIndex());
        }
    }
}
