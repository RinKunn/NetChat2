using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonServiceLocator;
using Autofac;
using GalaSoft.MvvmLight;
using Autofac.Extras.CommonServiceLocator;

namespace NetChat2.ViewModel
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            if (!ServiceLocator.IsLocationProviderSet)
            {
                RegisterServices();
            }
        }

        public static void RegisterServices(ContainerBuilder registrations = null, bool registerFakes = false)
        {
            var builder = new ContainerBuilder();
            builder.RegisterAppServices();
            var container = builder.Build();

            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(container));
        }

        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();
    }

    public static class LocatorServices
    {
        public static TService GetService<TService>(this IServiceLocator locator)
        {
            return (TService)locator.GetService(typeof(TService));
        }
    }
}
