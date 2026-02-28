using System.Collections.Generic;

namespace Game.Domain.Poe.Flasks
{
    public sealed class FlaskService
    {
        private readonly Dictionary<string, int> _charges = new();

        public void Initialize(FlaskDefinition flask)
        {
            _charges[flask.Id] = flask.MaxCharges;
        }

        public int GetCharges(string flaskId) => _charges.TryGetValue(flaskId, out var c) ? c : 0;

        public bool TryUse(FlaskDefinition flask)
        {
            int current = GetCharges(flask.Id);
            if (current < flask.ChargesPerUse)
                return false;

            _charges[flask.Id] = current - flask.ChargesPerUse;
            return true;
        }

        public void GainCharges(string flaskId, int amount, int maxCharges)
        {
            int current = GetCharges(flaskId);
            int next = current + amount;
            if (next > maxCharges) next = maxCharges;
            _charges[flaskId] = next;
        }
    }
}
