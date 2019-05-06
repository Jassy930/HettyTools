using System.Windows.Controls;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System;

namespace MetroDemo
{
    public partial class CommonTools : UserControl
    {
        public CommonTools()
        {
            InitializeComponent();
        }

        private void HFCfbox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                Single ff = Convert.ToSingle(HFCfbox.Text);
                var b = BitConverter.GetBytes(ff);
                Array.Reverse(b);
                HFChbox.Text = BitConverter.ToString(b).Replace('-',' ');
            }
            catch(Exception)
            {

            }
        }

        private void HFChbox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //try
            //{
                string s = HFChbox.Text;
                if(s.Substring(0,2) == "0x")
                {
                    s = s.Substring(2);
                }
                if(s.Length == 8)
                {
                    byte[] b = new byte[4];
                    b[0] = Convert.ToByte(s.Substring(0, 2), 16);
                    b[1] = Convert.ToByte(s.Substring(2, 2), 16);
                    b[2] = Convert.ToByte(s.Substring(4, 2), 16);
                    b[3] = Convert.ToByte(s.Substring(6, 2), 16);
                    float f = BitConverter.ToSingle(b,0);
                    HFCfbox.Text = f.ToString();
                }
            //}
            //catch(Exception)
            //{

            //}
        }
    }
}
