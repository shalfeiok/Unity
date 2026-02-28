namespace Game.Presentation.UI.Windowing
{
    public sealed class WindowModalBlocker
    {
        private int _modalDepth;

        public bool IsBlocking => _modalDepth > 0;

        public void PushModal() => _modalDepth++;

        public void PopModal()
        {
            if (_modalDepth > 0)
                _modalDepth--;
        }
    }
}
