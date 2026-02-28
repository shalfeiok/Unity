using System;
using System.Collections.Generic;

namespace Game.Application.Transactions
{
    public sealed class TransactionRunner
    {
        private readonly HashSet<string> _processed = new();

        public bool Run(string operationId, Func<bool> validate, Action apply, Action publish)
        {
            if (string.IsNullOrWhiteSpace(operationId))
                return false;

            if (_processed.Contains(operationId))
                return true;

            if (validate != null && !validate())
                return false;

            apply?.Invoke();
            publish?.Invoke();
            _processed.Add(operationId);
            return true;
        }
    }
}
