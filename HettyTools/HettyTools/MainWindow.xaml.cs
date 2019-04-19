using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro;
using MahApps.Metro.Controls;

namespace HettyTools
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void HamburgerMenuControl_OnItemInvoked(object sender, HamburgerMenuItemInvokedEventArgs e)
        {
            HamburgerMenuControl.Content = e.InvokedItem;
        }

        private void LaunchNewRelease(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Jassy930/HettyTools/releases/");
        }

        private void Theme_L_D_Click(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.BaseTheme == "Light")
            {
                ThemeManager.ChangeThemeBaseColor(Application.Current, "Dark");
                Properties.Settings.Default.BaseTheme = "Dark";
            }
            else
            {
                ThemeManager.ChangeThemeBaseColor(Application.Current, "Light");
                Properties.Settings.Default.BaseTheme = "Light";
            }
            Properties.Settings.Default.Save();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ThemeManager.ChangeThemeBaseColor(Application.Current, Properties.Settings.Default.BaseTheme);
        }
    }
}
