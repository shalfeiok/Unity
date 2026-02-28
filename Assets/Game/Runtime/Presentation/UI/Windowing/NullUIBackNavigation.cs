namespace Game.Presentation.UI.Windowing
{
    public sealed class NullUIBackNavigation : IUIBackNavigation
    {
        public static readonly NullUIBackNavigation Instance = new();

        private NullUIBackNavigation()
        {
        }

        public bool HasModal() => false;

        public bool TryCloseModal() => false;

        public bool TryCloseTopPanel() => false;
    }
}
