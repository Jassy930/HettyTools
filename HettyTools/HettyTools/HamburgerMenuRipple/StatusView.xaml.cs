using System;
using System.Windows.Controls;
using System.Windows.Threading;
using MahApps.Metro.Controls;

namespace MetroDemo
{
    /// <summary>
    /// Interaction logic for OtherExamples.xaml
    /// </summary>
    public partial class StatusView : UserControl
    {
        public StatusView()
        {
            InitializeComponent();

            //var t = new DispatcherTimer(TimeSpan.FromSeconds(2), DispatcherPriority.Normal, Tick, this.Dispatcher);
        }

        void Tick(object sender, EventArgs e)
        {
            var dateTime = DateTime.Now;
            transitioning.Content = new TextBlock { Text = "Transitioning Content! " + dateTime, SnapsToDevicePixels = true };
        }

        public void ShowStatus(string s)
        {
            transitioning.Content = new TextBlock { Text = s, SnapsToDevicePixels = true };
        }
    }
}
