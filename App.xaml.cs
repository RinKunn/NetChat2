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
        private IContainer _container;

        protected override void OnStartup(StartupEventArgs e)
        {
            logger.Info(new string('-', 20));
            base.OnStartup(e);
            InitializeInternal();
        }

        private void InitializeInternal()
        {
            _container = BuildAppContainer();
            MainWindow window = new MainWindow();
            using(var scope = _container.BeginLifetimeScope())
            {
                window.DataContext = _container.Resolve<MainViewModel>();
            }
            window.Show();
        }

        private IContainer BuildAppContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAppServices();
            return builder.Build();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            LogManager.Shutdown();
        }
    }
}
