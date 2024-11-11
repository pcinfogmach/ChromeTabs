using ChromeTabs.Helpers;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Media;

namespace ChromeTabs
{
    internal class ChromeTabsViewModel : INotifyPropertyChanged
    {
        #region InterFace
        public event PropertyChangedEventHandler? PropertyChanged;

        // Helper method to set a property and notify if it changes
        private void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName);
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        bool isLoaded;
        public ChromeTabsViewModel()
        {
            if (!isLoaded) LoadState(); isLoaded = true;
            Application.Current.Exit += async (s, e) => SaveState();
        }

        #region Serialization
        private string StateFilePath
        {
            get
            {
                string localeDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Locale");
                string cultureInfo = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

                string path = Path.Combine(localeDir, $"{cultureInfo}.json");
                if (File.Exists(path))  return path;

                // Fallback to en
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Locale", "en.json");
                if (File.Exists(path)) return path;

                // Try to find any file that matches a two-letter code format (e.g., "fr.json", "es.json")
                var matchingFile = Directory.EnumerateFiles(localeDir, "*.json")
                                            .FirstOrDefault(file => Path.GetFileName(file).Length == 7);
                if (matchingFile != null) return matchingFile;
                else return path;
            }
        }
        public void SaveState()
        {
            string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(StateFilePath, json);
        }
        public void LoadState()
        {
            if (File.Exists(StateFilePath))
            {
                string json = File.ReadAllText(StateFilePath);
                ChromeTabsViewModel loadedState = JsonSerializer.Deserialize<ChromeTabsViewModel>(json);

                if (loadedState != null)
                {
                    // Use reflection to assign each property from loadedState to this instance
                    foreach (PropertyInfo property in typeof(ChromeTabsViewModel).GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                        if (property.CanWrite)
                        {
                            property.SetValue(this, property.GetValue(loadedState));
                        }
                    }
                }

                bool isDarkTheme = ThemeHelper.IsDarkThemeEnabled();
                _background = new SolidColorBrush(isDarkTheme ? Color.FromRgb(30, 30, 30) : Color.FromRgb(200, 200, 200));
                _foreground = new SolidColorBrush(isDarkTheme ? Color.FromRgb(200, 200, 200) : Color.FromRgb(30, 30, 30));
                _flowDirection = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "he" ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
            }
        }
        #endregion

        #region Members
        SolidColorBrush _background;
        SolidColorBrush _foreground;
        FlowDirection _flowDirection;

        private string _maximizeButtonToolTip;
        private string _fullScreenButtonToolTip;
        private string _minimizeButtonToolTip;
        private string _xButtonToolTip;
        double? _top;
        double? _left;
        double _width;
        double _height;
        WindowState _windowState;
        int _selectedTabIndex; 

        [JsonIgnore]
        public SolidColorBrush Background { get => _background; set => SetProperty(ref _background, value); }
        [JsonIgnore]
        public SolidColorBrush Foreground { get => _foreground; set => SetProperty(ref _foreground, value); }
        [JsonIgnore]
        public FlowDirection FlowDirection{ get => _flowDirection; set => SetProperty(ref _flowDirection, value);}

        public string MinimizeButtonTooltip { get => _minimizeButtonToolTip; set => SetProperty(ref _minimizeButtonToolTip, value); }
        public string FullScreenButtonTooltip { get => _fullScreenButtonToolTip; set => SetProperty(ref _fullScreenButtonToolTip, value); }
        public string MaximizeButtonTooltip { get => _maximizeButtonToolTip; set => SetProperty(ref _maximizeButtonToolTip, value); }
        public string XButtonTooltip { get => _xButtonToolTip; set => SetProperty(ref _xButtonToolTip, value); }
        public double? Top { get => _top; set => SetProperty(ref _top, value); }
        public double? Left { get => _left; set => SetProperty(ref _left, value); }
        public double Width { get => _width; set => SetProperty(ref _width, value); }
        public double Height { get => _height; set => SetProperty(ref _height, value); }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public WindowState WindowState { get => _windowState; set => SetProperty(ref _windowState, value); }
        public int SelectedTabIndex { get => _selectedTabIndex; set => SetProperty(ref _selectedTabIndex, value); }
        #endregion
    }
}
