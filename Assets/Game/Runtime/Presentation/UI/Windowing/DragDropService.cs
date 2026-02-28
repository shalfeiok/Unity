namespace Game.Presentation.UI.Windowing
{
    public enum DragPayloadKind
    {
        None = 0,
        ItemRef = 1,
        GemRef = 2,
        SkillRef = 3,
        CurrencyRef = 4
    }

    public readonly struct DragPayload
    {
        public DragPayload(DragPayloadKind kind, string id)
        {
            Kind = kind;
            Id = id;
        }

        public DragPayloadKind Kind { get; }
        public string Id { get; }
    }

    public sealed class DragDropService
    {
        private DragPayload _current;

        public bool HasActivePayload => _current.Kind != DragPayloadKind.None;
        public DragPayload Current => _current;

        public void BeginDrag(DragPayload payload)
        {
            _current = payload;
        }

        public DragPayload EndDrag()
        {
            var payload = _current;
            _current = default;
            return payload;
        }

        public void CancelDrag() => _current = default;
    }
}
