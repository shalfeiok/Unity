using NUnit.Framework;
using Game.Domain.Tags;

namespace Game.Tests.EditMode
{
    public sealed class TagSetTests
    {
        [Test]
        public void Add_Contains_Works()
        {
            var set = new TagSet(4);
            var a = new TagId(1);
            var b = new TagId(2);

            Assert.False(set.Contains(a));
            set.Add(a);
            Assert.True(set.Contains(a));
            Assert.False(set.Contains(b));
        }
    }
}
