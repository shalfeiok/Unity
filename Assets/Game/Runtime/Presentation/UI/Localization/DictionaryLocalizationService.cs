using System.Collections.Generic;

namespace Game.Presentation.UI.Localization
{
    public sealed class DictionaryLocalizationService : ILocalizationService
    {
        private readonly IReadOnlyDictionary<string, string> _dictionary;

        public DictionaryLocalizationService(IReadOnlyDictionary<string, string> dictionary)
        {
            _dictionary = dictionary;
        }

        public string Translate(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return string.Empty;

            return _dictionary.TryGetValue(key, out var value) ? value : key;
        }

        public string TranslateFormat(string key, params object[] args)
        {
            var format = Translate(key);
            return string.Format(format, args);
        }
    }
}
