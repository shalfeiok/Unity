using Game.Application;
using Game.Domain;
using Game.Infrastructure;
using UnityEngine;

namespace Game.Runtime.Bootstrap
{
    public sealed class GameBootstrap : MonoBehaviour
    {
        public TransactionRunner Runner { get; private set; }

        private void Awake()
        {
            var clock = new FixedTickClock(30);
            var ledger = new InMemoryLedger();
            var eventBus = new InMemoryEventBus();
            Runner = new TransactionRunner(clock, ledger, eventBus);
        }
    }
}
