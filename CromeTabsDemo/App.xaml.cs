
using ChromeTabs;
using Microsoft.Web.WebView2.Wpf;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;

namespace CromeTabsDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ChromeTabs.ChromeTabsWindow window = new ChromeTabs.ChromeTabsWindow();
            window.Show();
            window.ChromeTabControl.Items.Add(new ChromeTabItem { Header = "תחומים", Content = new WebView2 { Source = new Uri("https://tchumim.com/") } });
            window.ChromeTabControl.Items.Add(new ChromeTabItem { Header = "מתמחים טופ", Content = new WebView2 { Source = new Uri("https://mitmachim.top/") } });
            window.ChromeTabControl.Items.Add(new ChromeTabItem { Header = "כל רגע", Content = new WebView2 { Source = new Uri("https://www.kore.co.il/") } });
            window.ChromeTabControl.Items.Add(new ChromeTabItem { Header = "קול הלשון", Content = new WebView2 { Source = new Uri("https://www2.kolhalashon.com/") } });
        }
    }

}
