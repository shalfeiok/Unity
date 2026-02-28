using System;

namespace Game.Domain.Tags
{
    /// <summary>
    /// Allocation-free small set of tags. For production you can swap the backing store (bitset / pooled hashset).
    /// This struct is intentionally minimal; expand carefully and keep hot-path alloc-free.
    /// </summary>
    public struct TagSet
    {
        private TagId[] _tags;
        private int _count;

        public TagSet(int capacity)
        {
            _tags = new TagId[Math.Max(1, capacity)];
            _count = 0;
        }

        public int Count => _count;

        public void Clear() => _count = 0;

        public bool Contains(TagId tag)
        {
            for (int i = 0; i < _count; i++)
                if (_tags[i].Value == tag.Value) return true;
            return false;
        }

        public void Add(TagId tag)
        {
            if (Contains(tag)) return;
            EnsureCapacity(_count + 1);
            _tags[_count++] = tag;
        }

        public void UnionWith(in TagSet other)
        {
            for (int i = 0; i < other._count; i++)
                Add(other._tags[i]);
        }

        private void EnsureCapacity(int needed)
        {
            if (_tags.Length >= needed) return;
            int newSize = _tags.Length * 2;
            if (newSize < needed) newSize = needed;
            Array.Resize(ref _tags, newSize);
        }
    }
}
