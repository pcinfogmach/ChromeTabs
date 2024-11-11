using ChromeTabs.Helpers;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;

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

        #region Serialization
        public void SaveState()
        {
            string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(StateFilePath(), json);
        }

        public void LoadState()
        {
            string stateFilePath = StateFilePath();
            if (File.Exists(stateFilePath))
            {
                string json = File.ReadAllText(stateFilePath);
                ChromeTabsViewModel loadedState = JsonSerializer.Deserialize<ChromeTabsViewModel>(json);

                if (loadedState != null)
                {
                    // Use reflection to assign each property from loadedState to this instance
                    foreach (PropertyInfo property in typeof(ChromeTabsViewModel).GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                        if (property.CanWrite)
                            property.SetValue(this, property.GetValue(loadedState));
                    }
                }
            }

            bool isDarkTheme = ThemeHelper.IsDarkThemeEnabled();
            WindowBackground = new SolidColorBrush(isDarkTheme ? Color.FromRgb(30, 30, 30) : Color.FromRgb(200, 200, 200));
            WindowForeground = new SolidColorBrush(isDarkTheme ? Color.FromRgb(200, 200, 200) : Color.FromRgb(30, 30, 30));
            WindowFlowDirection = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "he" ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
        }

        private string StateFilePath()
        {
            string localeDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Locale");
            string cultureInfo = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

            string path = Path.Combine(localeDir, $"{cultureInfo}.json");
            if (File.Exists(path)) return path;

            // Fallback to en
            path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Locale", "en.json");
            if (File.Exists(path)) return path;

            // Try to find any file that matches a two-letter code format (e.g., "fr.json", "es.json")
            var matchingFile = Directory.EnumerateFiles(localeDir, "*.json")
                                        .FirstOrDefault(file => Path.GetFileName(file).Length == 7);
            if (matchingFile != null) return matchingFile;
            else return string.Empty;
        }
        #endregion

        #region Members
        [JsonIgnore] SolidColorBrush _windowBackground;
        [JsonIgnore] SolidColorBrush _windowForeground;
        [JsonIgnore] FlowDirection _windowFlowDirection;

        private string _maximizeButtonToolTip;
        private string _fullScreenButtonToolTip;
        private string _minimizeButtonToolTip;
        private string _xButtonToolTip;
        double? _windowTop;
        double? _windowLeft;
        double? _windowWidth;
        double? _windowHeight;
        WindowState _windowState;
        int _selectedTabIndex;

        [JsonIgnore] public SolidColorBrush WindowBackground { get => _windowBackground; set => SetProperty(ref _windowBackground, value); }
        [JsonIgnore] public SolidColorBrush WindowForeground { get => _windowForeground; set => SetProperty(ref _windowForeground, value); }
        [JsonIgnore] public FlowDirection WindowFlowDirection { get => _windowFlowDirection; set => SetProperty(ref _windowFlowDirection, value); }
        public string MinimizeButtonTooltip { get => _minimizeButtonToolTip;  set => SetProperty(ref _minimizeButtonToolTip, value); }
        public string FullScreenButtonTooltip { get => _fullScreenButtonToolTip; set => SetProperty(ref _fullScreenButtonToolTip, value); }
        public string MaximizeButtonTooltip { get => _maximizeButtonToolTip; set => SetProperty(ref _maximizeButtonToolTip, value); }
        public string XButtonTooltip { get => _xButtonToolTip; set => SetProperty(ref _xButtonToolTip, value); }
        public double? WindowTop { get => _windowTop; set => SetProperty(ref _windowTop, value); }
        public double? windowLeft { get => _windowLeft; set => SetProperty(ref _windowLeft, value); }
        public double? WindowWidth { get => _windowWidth; set => SetProperty(ref _windowWidth, value); }
        public double? WindowHeight { get => _windowHeight; set => SetProperty(ref _windowHeight, value); }
        [JsonConverter(typeof(JsonStringEnumConverter))] public WindowState WindowState { get => _windowState; set => SetProperty(ref _windowState, value); }
        public int SelectedTabIndex { get => _selectedTabIndex; set => SetProperty(ref _selectedTabIndex, value); }
        #endregion

        #region Commands
        public ICommand LoadStateCommand { get => new RelayCommand(LoadState); }
        public ICommand SaveStateCommand { get => new RelayCommand(SaveState); }
        #endregion
    }
}
