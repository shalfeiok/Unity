using System;
using System.Collections.Generic;

namespace Game.Domain.Simulation
{
    public sealed class SimulationLoop
    {
        private readonly List<Action<SimTime>> _systems = new();
        private SimAccumulator _accumulator;
        private SimTime _simTime;

        public SimulationLoop(float fixedDeltaTime)
        {
            _accumulator = new SimAccumulator();
            _simTime = new SimTime(0, fixedDeltaTime);
        }

        public SimTime CurrentTime => _simTime;

        public void Register(Action<SimTime> system)
        {
            if (system == null) throw new ArgumentNullException(nameof(system));
            _systems.Add(system);
        }

        public int Step(float frameDeltaTime, int maxTicksPerFrame = 8)
        {
            _accumulator.AddTime(frameDeltaTime);
            int ticks = 0;

            while (_accumulator.TryConsume(_simTime.DeltaTime) && ticks < maxTicksPerFrame)
            {
                ExecuteTick(_simTime);
                _simTime = _simTime.NextTick();
                ticks++;
            }

            return ticks;
        }

        private void ExecuteTick(SimTime simTime)
        {
            for (int i = 0; i < _systems.Count; i++)
                _systems[i](simTime);
        }

        private struct SimAccumulator
        {
            private float _value;

            public void AddTime(float deltaTime)
            {
                if (deltaTime <= 0f) return;
                _value += deltaTime;
            }

            public bool TryConsume(float step)
            {
                if (_value < step) return false;
                _value -= step;
                return true;
            }
        }
    }
}
