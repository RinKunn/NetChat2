using System;
using System.Diagnostics;
using Autofac;
using NetChat2.Connector;
using NetChat2.Models;
using NetChat2.Services;
using NetChat2.ViewModel;
using NetChat2.Persistance;

namespace NetChat2
{
    public static class ServiceRegistration
    {
        public static ContainerBuilder RegisterAppServices(this ContainerBuilder builder, string chatsPath = null, string usersPath = null)
        {
            string ChatSourcePath = chatsPath ?? System.Configuration.ConfigurationManager.AppSettings["ChatSourcePath"];
            string UserSourcePath = usersPath ?? System.Configuration.ConfigurationManager.AppSettings["UserSourcePath"];

            builder
                .RegisterType<JsonChatRepository>()
                .As<IChatRepository>()
                .WithParameter(new TypedParameter(typeof(string), ChatSourcePath))
                .InstancePerLifetimeScope();
            builder
                .RegisterType<JsonUserRepository>()
                .As<IUserRepository>()
                .WithParameter(new TypedParameter(typeof(string), UserSourcePath))
                .InstancePerLifetimeScope();

            builder.RegisterType<DefaultUserService>().As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<DefaultMessageService>().As<IMessageService>().InstancePerLifetimeScope();
            builder.RegisterType<DefaultChatLoader>().As<IChatLoader>().InstancePerLifetimeScope();
            builder.RegisterType<DefaultMessageHub>().As<IMessageHub>().SingleInstance();

            builder.RegisterType<MainViewModel>();

            return builder;
        }
    }
}
