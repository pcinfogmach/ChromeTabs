

namespace LocaleTest
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class TranslationConverter : IValueConverter
    {
        public TranslationService TranslationService { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is string key && TranslationService != null)
            {
                return TranslationService.Translate(key);
            }
            return $"[{parameter}]";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


}
