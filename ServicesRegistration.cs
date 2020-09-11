using System;
using System.Diagnostics;
using Autofac;
using NetChat2.FileMessaging;
using NetChat2.Models;
using NetChat2.Services;
using NetChat2.ViewModel;

namespace NetChat2
{
    public static class ServicesRegistration
    {
        public static ContainerBuilder RegisterAppServices(this ContainerBuilder builder)
        {
            //builder.RegisterType<ChatService>().As<IMessengerService>().SingleInstance();
            builder.RegisterType<FileNetchatHub>()
                .As<INetchatHub>()
                .WithParameter(new TypedParameter(typeof(string), System.Configuration.ConfigurationManager.AppSettings["MessagesPath"]))
                .InstancePerLifetimeScope();
            builder.RegisterType<MainViewModel>();

            var processesCount = Process.GetProcessesByName("NetChat2").Length;
            var user = new User();
            builder.RegisterInstance<User>(user);
            return builder;
        }
    }
}
