namespace LocaleTest
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class MainViewModel : INotifyPropertyChanged
    {
        public TranslationService TranslationService { get; } = new TranslationService();

        private string _languageCode;
        public string LanguageCode
        {
            get => _languageCode;
            set
            {
                _languageCode = value;
                TranslationService.LoadTranslations(_languageCode);
                OnPropertyChanged();
            }
        }

        public string TranslationKey { get; set; } // Dummy property for binding purposes

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }


}
