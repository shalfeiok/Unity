using System;
using System.Collections.Generic;

namespace Game.Presentation.UI.Windowing
{
    public sealed class UIRefreshScheduler
    {
        private readonly Queue<Action> _queue = new();

        public int PendingCount => _queue.Count;

        public void Enqueue(Action refreshAction)
        {
            if (refreshAction == null) return;
            _queue.Enqueue(refreshAction);
        }

        public int ExecuteBudgeted(int maxActionsPerFrame)
        {
            if (maxActionsPerFrame <= 0) return 0;

            int executed = 0;
            while (_queue.Count > 0 && executed < maxActionsPerFrame)
            {
                _queue.Dequeue().Invoke();
                executed++;
            }

            return executed;
        }

        public void Clear() => _queue.Clear();
    }
}
