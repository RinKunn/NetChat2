using System.Windows;
using Autofac;
using NetChat2.ViewModel;
using NLog;

namespace NetChat2
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();


        protected override void OnStartup(StartupEventArgs e)
        {
            logger.Info(new string('-', 20));
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            LogManager.Shutdown();
        }
    }
}
