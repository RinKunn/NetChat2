using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Threading;
using NetChat2.ViewModel;

namespace NetChat2
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IContainer _container;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            InitializeInternal();
        }

        private void InitializeInternal()
        {
            _container = BuildAppContainer();
            //ServiceLocator.SetLocatorProvider(() => _container.Resolve<IServiceLocator>());

            MainWindow window = new MainWindow();
            window.DataContext = _container.Resolve<MainViewModel>();
            window.Show();
        }

        private IContainer BuildAppContainer()
        {
            var builder = new ContainerBuilder();
            //builder.RegisterInstance(_container);
            //builder.RegisterType<IServiceLocator>().As<AutofacServiceLocator>().SingleInstance();
            builder.RegisterAppServices();
            return builder.Build();
        }
    }
}
