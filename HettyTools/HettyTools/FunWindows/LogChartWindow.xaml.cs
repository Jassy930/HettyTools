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
using System.Windows.Shapes;
using MahApps.Metro;
using MahApps.Metro.Controls;
using LiveCharts;
using LiveCharts.Wpf;
using System.IO;
using System.ComponentModel;
using System.Globalization;

namespace HettyTools.FunWindows
{
    /// <summary>
    /// Interaction logic for LogChartWindow.xaml
    /// </summary>
    public partial class LogChartWindow : MetroWindow
    {
        string logfilename = string.Empty;
        logdata ld = new logdata();

        SeriesCollection chartseries = new SeriesCollection
        {
            new LineSeries
            {
                Values = new ChartValues<double> { 3, 5, 7, 4 }
        },
            new ColumnSeries
            {
                Values = new ChartValues<decimal> { 5, 6, 2, 7 }
            }
        };
        public LogChartWindow(string s)
        {
            InitializeComponent();
            this.DataContext = ld;
            chart.Series = chartseries;
            logfilename = s;
            Handlelogfile();
        }

        public void setlogfilename(string s)
        {
            logfilename = s;
        }

        private void Handlelogfile()
        {
            if (!File.Exists(logfilename)) return;
            StreamReader sr = new StreamReader(logfilename);
            DateTime zerotime = DateTime.Now;
            while (!sr.EndOfStream)
            {
                string ll = sr.ReadLine();
                if (ll.Contains("Log File Version"))
                {
                    ld.logfileversion = ll;
                    ll = sr.ReadLine();
                    ld.logdate = ll;
                }
                else if(ll.Contains("---------------"))
                {
                    ll = sr.ReadLine();
                    ld.dataname = new List<string>( ll.Split(','));
                    ld.dataname.RemoveRange(0, 2);
                    ll = sr.ReadLine();
                    ld.datatype = new List<string>( ll.Split(','));
                    ld.datatype.RemoveRange(0, 2);
                    ll = sr.ReadLine();
                    ld.datanum = ld.datatype.Count;
                    ld.data = new List<List<double>>(ld.datanum-2);
                    for (int i = 0; i < ld.datanum; i++) ld.data.Add(new List<double>());
                    while (!sr.EndOfStream)
                    {
                        ll = sr.ReadLine();
                        string[] dd = ll.Split(',');
                        DateTime dt = DateTime.ParseExact(dd[1], "MM/dd/yyyy hh:mm:ss.fff", null);
                        if(ld.timeseries.Count == 0)
                        {
                            zerotime = dt;
                            ld.timeseries.Add(0);
                        }
                        else
                        {
                            ld.timeseries.Add((dt - zerotime).TotalSeconds);
                        }
                        for(int i=2; i<dd.Length; i++)
                        {
                            if (ld.datatype[i-2] == "HEX")
                                ld.data[i - 2].Add(HEX2float(dd[i]));
                            else if(ld.datatype[i-2] == "None")
                                ld.data[i - 2].Add(Convert.ToDouble(dd[i]));
                            else if (ld.datatype[i-2] == "RPM")
                                ld.data[i - 2].Add(Convert.ToDouble(dd[i]));
                        }
                    }
                }
            }
            sr.Close();

            ld.vislist = new List<vislistclass>();
            foreach (string s in ld.dataname)
            {
                ld.vislist.Add(new vislistclass(s));
            }

            chart.Series = getseries();
        }

        private SeriesCollection getseries()
        {
            SeriesCollection chartseries = new SeriesCollection();


            for (int i = 0; i < ld.datanum; i++)
            {
                if (i != 3 && i!=6)
                {
                    var ls = new LineSeries
                    {
                        Values = new ChartValues<double>(ld.data[i]),
                        Fill = Brushes.Transparent,
                        StrokeThickness = 2.5,
                        PointGeometry = null,
                        Title = ld.dataname[i],
                    };
                    Binding bb = new Binding("visb");
                    ls.SetBinding(LineSeries.VisibilityProperty, bb);
                    chartseries.Add(ls);
                }
            }

            return chartseries;
        }

        private float HEX2float(string s)
        {
            try
            {
                if (s.Substring(0, 2) == "0x")
                {
                    s = s.Substring(2);
                }
                if (s.Length == 8)
                {
                    byte[] b = new byte[4];
                    b[3] = Convert.ToByte(s.Substring(0, 2), 16);
                    b[2] = Convert.ToByte(s.Substring(2, 2), 16);
                    b[1] = Convert.ToByte(s.Substring(4, 2), 16);
                    b[0] = Convert.ToByte(s.Substring(6, 2), 16);
                    return BitConverter.ToSingle(b, 0);
                }
            }
            catch (Exception)
            {

            }
            return 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            chart.Series = getseries();
        }
    }

    class logdata : INotifyPropertyChanged
    {
        public logdata()
        {
            timeseries = new List<double>();
        }


        public string logfileversion;
        public string logdate;
        public List<string> dataname;
        public List<double> timeseries;
        public List<string> datatype;
        public List<List<double>> data;
        public int datanum;

        private List<string> SeriesVisibilityvalue;

        public List<string> SeriesVisibility
        {
            get
            {
                return SeriesVisibilityvalue;
            }
            set
            {
                if (value != SeriesVisibilityvalue)
                {
                    SeriesVisibilityvalue = value;
                }

                NotifyPropertyChanged("SeriesVisibility");
            }
        }

        private List<vislistclass> vislistvalue;

        public List<vislistclass> vislist
        {
            get
            {
                return vislistvalue;
            }
            set
            {
                if(value!=vislistvalue)
                {
                    vislistvalue = value;
                }
                NotifyPropertyChanged("vislist");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }

    public class vislistclass : INotifyPropertyChanged
    {
        private string datanamevalue;

        public string dataname
        {
            get
            {
                return datanamevalue;
            }
            set
            {
                if (value != datanamevalue)
                {
                    datanamevalue = value;
                }

                NotifyPropertyChanged("dataname");
            }
        }

        private bool visvalue;

        public bool vis
        {
            get
            {
                return visvalue;
            }
            set
            {
                if (value != visvalue)
                {
                    visvalue = value;
                    if (value) visb = Visibility.Visible;
                    else visb = Visibility.Collapsed;
                }

                NotifyPropertyChanged("vis");
            }
        }

        private Visibility visbvalue;

        public Visibility visb
        {
            get
            {
                return visbvalue;
            }
            set
            {
                if (value != visbvalue)
                {
                    visbvalue = value;
                }

                NotifyPropertyChanged("visb");
            }
        }


        public vislistclass(string name)
        {
            datanamevalue = name;
            visvalue = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }

    public class VisibilityConver : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return DependencyProperty.UnsetValue;
            }
            if ((bool)value)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Hidden;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    public class ObservableValue : IObservableChartPoint
    {
        private double _value;
        public ObservableValue()
        {

        }

        public ObservableValue(double value)
        {
            Value = value;
        }

        public event Action PointChanged;
        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPointChanged();
            }
        }

        protected void OnPointChanged()
        {
            if (PointChanged != null) PointChanged.Invoke();
        }
    }

}
