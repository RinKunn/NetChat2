using System;
using System.Diagnostics;
using Autofac;
using NetChat2.Connector;
using NetChat2.Models;
using NetChat2.Services;
using NetChat2.ViewModel;

namespace NetChat2
{
    public static class DIRegistration
    {
        public static ContainerBuilder RegisterAppServices(this ContainerBuilder builder)
        {
            builder.RegisterType<ChatService>().As<IChatService>().SingleInstance();
            builder.RegisterType<FileNetchatHub>()
                .As<INetchatHub>()
                .WithParameter(new TypedParameter(typeof(string), System.Configuration.ConfigurationManager.AppSettings["MessagesPath"]))
                .InstancePerLifetimeScope();
            builder.RegisterType<MainViewModel>();

            var processesCount = Process.GetProcessesByName("NetChat2").Length;
            var user = new User(Environment.UserName.ToUpper() + (processesCount > 1 ? $"_{processesCount}" : string.Empty));
            builder.RegisterInstance<User>(user);
            return builder;
        }
    }
}
