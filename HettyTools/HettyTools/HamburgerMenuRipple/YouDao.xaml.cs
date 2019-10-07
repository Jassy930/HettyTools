using System.Windows.Controls;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System;
using HettyTools.FunWindows;
using System.Net;
using System.Threading;

namespace HettyTools
{
    public partial class YouDao : UserControl
    {
        public YouDao()
        {
            InitializeComponent();
        }

        private void WordBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create("http://dict.youdao.com/w/eng/" + this.WordBox.Text);
            req.Method = "GET";
            using (WebResponse wr = req.GetResponse())
            {
                using (Stream reqstream = wr.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(reqstream, Encoding.UTF8))
                    {
                        string htmlcontent = sr.ReadToEnd().ToString();
                        string r = @"<div id=""doc""[\s\S]*</iframe></div>?";
                        string result = Regex.Match(htmlcontent, @"<div id=""results"">[\s\S]*<div id=""ads?").Value;
                        result = result.Substring(0, result.Length - 12);
                        htmlcontent = Regex.Replace(htmlcontent, @"<body[\s\S]*/body>", "<body class=\"t0\">"+result+"</body>");
                        CefSharp.WebBrowserExtensions.LoadHtml(bws, htmlcontent, "http://www.baidu.com");
                    }
                }
            }
        }

        public void Word_Search(string s)
        {
            if (!bws.IsBrowserInitialized)
            {
                new Thread(() =>
                {
                    Thread.Sleep(100);
                    this.WordBox.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        Word_Search(s);
                    }));
                }).Start();
            }
            else
            {
                this.WordBox.Text = s;
            }
        }

    }
}
