﻿using System;
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
using System.Timers;
using System.IO;
using System.Threading;
using System.Data.SQLite;

namespace HettyTools
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        HTViewModel ht = new HTViewModel();
        NotifyIcon notifyIcon;
        System.Timers.Timer GREtimer = new System.Timers.Timer();
        List<string> GREwords = new List<string>();
        static string DBPath = "db";
        SQLiteConnection dbconn = null;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = ht;
            //NotifyIcon_Init();

            //AutoUpdater.ReportErrors = true;
            AutoUpdater.ShowSkipButton = false;
            AutoUpdater.ShowRemindLaterButton = false;
            AutoUpdater.Mandatory = true;
            AutoUpdater.UpdateMode = Mode.Forced;

            //GREtimer

            GREtimer.Elapsed += new ElapsedEventHandler(GRETimerHandle);
            GREtimer.Interval = 1000;
            GREtimer.Enabled = true;
            GREtimer.Start();

            StreamReader sr = new StreamReader("gre.words");
            string line = string.Empty;
            line = sr.ReadLine();
            while (line!= null)
            {
                GREwords.Add(line);
                line = sr.ReadLine();
            }
            sr.Close();
        }

        #region notifyIcon
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
        #endregion

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ThemeManager.ChangeTheme(System.Windows.Application.Current, Properties.Settings.Default.BaseTheme, Properties.Settings.Default.Accent);
            AutoUpdater.Start("https://blog.jassy.wang/release/Version.xml");
        }

        private void HamburgerMenuControl_OnItemInvoked(object sender, HamburgerMenuItemInvokedEventArgs e)
        {
            HamburgerMenuControl.Content = e.InvokedItem;
        }

        private void LaunchNewRelease(object sender, RoutedEventArgs e)
        {
            AutoUpdater.Start("https://blog.jassy.wang/release/Version.xml");
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

        int grecounter = 0;
        private void GRETimerHandle(object sender, ElapsedEventArgs e)
        {
            grecounter++;
            if (grecounter >= Properties.Settings.Default.GRErefreshtime)
            {
                grecounter = 0;
                GREnextHandle(null, null);
            }
        }

        private void GREyoudao_Click(object sender, RoutedEventArgs e)
        {
            HamburgerMenuControl.SelectedIndex = 3;
            youdaopage.Word_Search(GREbar.Text.Split(' ')[0]);
            //System.Diagnostics.Process.Start("http://dict.youdao.com/w/eng/"+GREbar.Text.Split(' ')[0]);
        }

        private void GREnextHandle(object sender, EventArgs e)
        {
            //TODO: reset timer of GREtimer
            Random r = new Random();
            int k = r.Next(0, GREwords.Count);

            new Thread(() =>
            {
                this.GREbar.Dispatcher.BeginInvoke(new Action(() =>
                {
                    GREbar.Text = GREwords[k];
                }));
            }).Start();
        }

        private void MetroWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (false && e.Key.ToString() == "F9")
            {
                if (this.dbconn == null)
                {
                    if (!string.IsNullOrEmpty(DBPath) && !Directory.Exists(DBPath))
                    {
                        Directory.CreateDirectory(DBPath);
                    }
                    var dbFilePath = System.IO.Path.Combine(DBPath, "database.db");
                    this.dbconn = new SQLiteConnection("DataSource = "+dbFilePath);
                }
                if (this.dbconn.State != System.Data.ConnectionState.Open)
                {
                    this.dbconn.Open();
                }
                
                using (var tr = this.dbconn.BeginTransaction())
                {
                    using (var cmd = this.dbconn.CreateCommand())
                    {
                        cmd.CommandText = "";
                        cmd.ExecuteNonQuery();
                    }
                    tr.Commit();
                }
                int b = 0;
                int a = 10 / b;
                System.Windows.MessageBox.Show(a.ToString());

            }
        }
    }
}
