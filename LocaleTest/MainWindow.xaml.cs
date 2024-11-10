using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LocaleTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            _viewModel = new MainViewModel();
            DataContext = _viewModel;

            // Set up TranslationConverter with the ViewModel's TranslationService
            var translationConverter = (TranslationConverter)Resources["TranslationConverter"];
            translationConverter.TranslationService = _viewModel.TranslationService;
        }

        private void SetEnglishLanguage(object sender, RoutedEventArgs e)
        {
            _viewModel.LanguageCode = "en";
        }

        private void SetHebrewLanguage(object sender, RoutedEventArgs e)
        {
            _viewModel.LanguageCode = "he";
        }
    }
}