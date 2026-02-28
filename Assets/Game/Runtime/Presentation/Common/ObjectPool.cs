using System;
using System.Collections.Generic;

namespace Game.Presentation.Common
{
    public sealed class ObjectPool<T> where T : class
    {
        private readonly Stack<T> _pool;
        private readonly Func<T> _factory;
        private readonly Action<T> _onGet;
        private readonly Action<T> _onRelease;

        public ObjectPool(int initialCapacity, Func<T> factory, Action<T> onGet = null, Action<T> onRelease = null)
        {
            _pool = new Stack<T>(Math.Max(1, initialCapacity));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _onGet = onGet;
            _onRelease = onRelease;

            for (int i = 0; i < initialCapacity; i++)
                _pool.Push(_factory());
        }

        public int Count => _pool.Count;

        public T Get()
        {
            var instance = _pool.Count > 0 ? _pool.Pop() : _factory();
            _onGet?.Invoke(instance);
            return instance;
        }

        public void Release(T instance)
        {
            if (instance == null) return;
            _onRelease?.Invoke(instance);
            _pool.Push(instance);
        }

        public void Warmup(int count)
        {
            while (_pool.Count < count)
                _pool.Push(_factory());
        }
    }
}
