using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HettyTools
{
    class HTViewModel : INotifyPropertyChanged
    {
        public HTViewModel()
        {
            temp_value = "hahahah";
        }

        private string temp_value;
        public string temp
        {
            get
            {
                return this.temp_value;
            }
            set
            {
                if (value != this.temp_value)
                {
                    temp_value = value;
                    NotifyPropertyChanged("temp");
                }
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
}
