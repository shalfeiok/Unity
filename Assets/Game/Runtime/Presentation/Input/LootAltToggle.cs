namespace Game.Presentation.Input
{
    public sealed class LootAltToggle
    {
        private bool _alwaysVisible;
        private bool _isAltHeld;

        public bool IsLootVisible => _alwaysVisible || _isAltHeld;

        public void SetAlwaysVisible(bool value) => _alwaysVisible = value;
        public void SetAltHeld(bool value) => _isAltHeld = value;
    }
}
