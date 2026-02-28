using Game.Presentation.Common;
using NUnit.Framework;

namespace Game.Tests.EditMode
{
    public sealed class ObjectPoolTests
    {
        [Test]
        public void GetRelease_ReusesInstances()
        {
            var pool = new ObjectPool<object>(0, static () => new object());

            var first = pool.Get();
            pool.Release(first);
            var second = pool.Get();

            Assert.AreSame(first, second);
        }
    }
}
