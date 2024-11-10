

namespace LocaleTest
{
    using System.Collections.Generic;
    using System.IO;

    public class TranslationService
    {
        private Dictionary<string, string> _translations = new Dictionary<string, string>();

        public void LoadTranslations(string languageCode)
        {
            string filePath = $"{languageCode}.txt";
            _translations.Clear();

            if (File.Exists(filePath))
            {
                foreach (var line in File.ReadLines(filePath))
                {
                    var parts = line.Split('=');
                    if (parts.Length == 2)
                    {
                        _translations[parts[0].Trim()] = parts[1].Trim();
                    }
                }
            }
        }

        public string Translate(string key)
        {
            return _translations.TryGetValue(key, out var value) ? value : $"[{key}]";
        }
    }

}
