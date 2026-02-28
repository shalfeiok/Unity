using System;
using System.Collections.Generic;

namespace Game.Presentation.UI.Windowing
{
    public sealed class VirtualizedListView<T>
    {
        private readonly IList<T> _source;

        public VirtualizedListView(IList<T> source)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
        }

        public int Count => _source.Count;

        public IReadOnlyList<T> GetVisibleRange(int firstIndex, int visibleCount)
        {
            if (visibleCount <= 0 || firstIndex >= _source.Count)
                return Array.Empty<T>();

            if (firstIndex < 0)
                firstIndex = 0;

            int max = Math.Min(_source.Count, firstIndex + visibleCount);
            int length = max - firstIndex;
            var result = new T[length];
            for (int i = 0; i < length; i++)
                result[i] = _source[firstIndex + i];

            return result;
        }
    }
}
