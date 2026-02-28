namespace Game.Presentation.UI.Localization
{
    public interface ILocalizationService
    {
        string Translate(string key);
        string TranslateFormat(string key, params object[] args);
    }
}
