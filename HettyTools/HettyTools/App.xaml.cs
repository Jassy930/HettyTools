using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace HettyTools
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.Startup += new StartupEventHandler(App_Startup);
        }

        void App_Startup(object sender, StartupEventArgs e)
        {
            this.DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }

        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                e.Handled = true;
                MessageBox.Show("Error_Dispatcher: " + e.Exception.Message+"\nSource: "+e.Exception.Source+"\nStackTrace: "+e.Exception.StackTrace);
            }
            catch(Exception ex)
            {
                MessageBox.Show("I can't hold it.\nError_Dispatcher");
            }
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string s = string.Empty;
            if (e.IsTerminating)
            {
                s+= "I can't hold it.\nError_Dispatcher";
            }
            s+= "Error_currentdomain:";
            if (e.ExceptionObject is Exception)
            {
                s+=(((Exception)e.ExceptionObject).Message);
            }
            else
            {
                s+=(e.ExceptionObject);
            }
            MessageBox.Show(s);
        }

        void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            //task线程内未处理捕获
            MessageBox.Show("Error_taskscheduler：" + e.Exception.Message);
            e.SetObserved();
        }

    }
}
