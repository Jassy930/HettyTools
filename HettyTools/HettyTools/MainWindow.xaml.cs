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
using System.Windows.Forms;
using AutoUpdaterDotNET;

namespace HettyTools
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        HTViewModel ht = new HTViewModel();
        NotifyIcon notifyIcon;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = ht;
            //NotifyIcon_Init();

            AutoUpdater.ReportErrors = true;
            AutoUpdater.ShowSkipButton = false;
            AutoUpdater.ShowRemindLaterButton = false;
            AutoUpdater.Mandatory = true;
            AutoUpdater.UpdateMode = Mode.Forced;
        }

        private void NotifyIcon_Init()
        {
            SystemTrayParameter pars = new SystemTrayParameter("ico/cat_black.ico", "Standing by", "", 0, notifyIcon_MouseDoubleClick);
            this.notifyIcon = SystemTray.SetSystemTray(pars, GetList());
            this.notifyIcon.Visible = true;
            //WinCommon.WinBaseSet(this);
        }

        private List<SystemTrayMenu> GetList()
        {
            List<SystemTrayMenu> ls = new List<SystemTrayMenu>();
            ls.Add(new SystemTrayMenu() { Txt = "打开主面板", Icon = "", Click = mainWin_Click });
            ls.Add(new SystemTrayMenu() { Txt = "退出", Icon = "ico/cat_mb.ico", Click = exit_Click });
            return ls;
        }

        void notifyIcon_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.Show();
            //this.notifyIcon.Visible = false;
        }

        void mainWin_Click(object sender, EventArgs e)
        {
            this.Show();
            //this.notifyIcon.Visible = false;
        }

        void exit_Click(object sender, EventArgs e)
        {
            this.Close();
            System.Windows.Application.Current.Shutdown();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ThemeManager.ChangeTheme(System.Windows.Application.Current, Properties.Settings.Default.BaseTheme, Properties.Settings.Default.Accent);
        }

        private void HamburgerMenuControl_OnItemInvoked(object sender, HamburgerMenuItemInvokedEventArgs e)
        {
            HamburgerMenuControl.Content = e.InvokedItem;
        }

        private void LaunchNewRelease(object sender, RoutedEventArgs e)
        {
            AutoUpdater.Start("http://127.0.0.1:4000/assets/release/Version.xml");
            //System.Diagnostics.Process.Start("https://github.com/Jassy930/HettyTools/releases/");
        }

        private void Theme_L_D_Click(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.BaseTheme == "Light")
            {
                ThemeManager.ChangeThemeBaseColor(System.Windows.Application.Current, "Dark");
                Properties.Settings.Default.BaseTheme = "Dark";
            }
            else
            {
                ThemeManager.ChangeThemeBaseColor(System.Windows.Application.Current, "Light");
                Properties.Settings.Default.BaseTheme = "Light";
            }
            Properties.Settings.Default.Save();
        }
    }
}
