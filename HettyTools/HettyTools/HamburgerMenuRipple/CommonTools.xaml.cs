using System.Windows.Controls;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System;
using HettyTools.FunWindows;

namespace HettyTools
{
    public partial class CommonTools : UserControl
    {
        bool HFCchangelock = false;
        LogChartWindow logchart;
        public CommonTools()
        {
            InitializeComponent();

            //funwindows
            //logchart = new LogChartWindow("");
            //logchart.ApplyTemplate();
            //logchart.Closed += (o, e) => logchart = null;

            //Closing += (s, e) =>
            //{
            //    if (!e.Cancel && logchart != null)
            //    {
            //        logchart.Close();
            //    }
            //};
        }

        private void HFChbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (HFCchangelock) return;
            try
            {
                HFCchangelock = true;
                string s = HFChbox.Text;
                 if(s.Substring(0,2) == "0x")
                {
                    s = s.Substring(2);
                }
                if(s.Length == 8)
                {
                    byte[] b = new byte[4];
                    b[3] = Convert.ToByte(s.Substring(0, 2), 16);
                    b[2] = Convert.ToByte(s.Substring(2, 2), 16);
                    b[1] = Convert.ToByte(s.Substring(4, 2), 16);
                    b[0] = Convert.ToByte(s.Substring(6, 2), 16);
                    float f = BitConverter.ToSingle(b,0);
                    HFCfbox.Text = f.ToString();
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                HFCchangelock = false;
            }
        }

        private void HFCfbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (HFCchangelock) return;
            try
            {
                HFCchangelock = true;
                Single ff = Convert.ToSingle(HFCfbox.Text);
                var b = BitConverter.GetBytes(ff);
                Array.Reverse(b);
                HFChbox.Text = BitConverter.ToString(b).Replace("-","");
            }
            catch(Exception)
            {

            }
            finally
            {
                HFCchangelock = false;
            }
        }

        private void LaunchLogChartWindow(object sender, RoutedEventArgs e)
        {
            if (logchart == null)
            {
                logchart = new LogChartWindow("C:\\Users\\Lenovo\\Documents\\code\\hetty\\test_file\\57_58_79.log.log.csv");
                //logchart.setlogfilename("C:\\Users\\Lenovo\\Documents\\code\\hetty\\test_file\\57_58_79.log.log.csv");
                logchart.Closed += (o, args) => logchart = null;
            }
            logchart.Show();
        }
    }
}
