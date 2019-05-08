using MahApps.Metro;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using System;
using System.Windows.Input;

namespace HettyTools
{
    /// <summary>
    /// Interaction logic for AboutView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();

            tb_Version.Text="Version:  " +System.Windows.Application.ResourceAssembly.GetName().Version.ToString();
        }

        private void ChangeThemeStyle(object sender, System.Windows.RoutedEventArgs e)
        {
            Tile tt = sender as Tile;
            HettyTools.Properties.Settings.Default.Accent = tt.Tag.ToString();
            HettyTools.Properties.Settings.Default.Save();
            ThemeManager.ChangeThemeColorScheme(Application.Current, tt.Tag.ToString());
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ThemesPanel.Children.Clear();
            foreach (ColorScheme o in ThemeManager.ColorSchemes)
            {
                Tile tt = new Tile();
                tt.Click += ChangeThemeStyle;
                tt.Tag = o.Name;
                tt.Background = o.ShowcaseBrush;
                tt.Width = 25;
                tt.Height = 25;
                ThemesPanel.Children.Add(tt);
            }
        }
    }
}
