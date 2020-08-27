using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using NetChat2.Services;
using NetChat2.Connector;
using NetChat2.ViewModel;
using System.IO;

namespace NetChat2
{
    public static class DIRegistration
    {
        public static ContainerBuilder RegisterAppServices(this ContainerBuilder builder)
        {
            builder.RegisterType<ChatService>().As<IChatService>().SingleInstance();
            builder.RegisterType<FileNetchatHub>()
                .As<INetchatHub>()
                .WithParameter(new TypedParameter(typeof(string), @"./netchattt2.txt"))
                .InstancePerLifetimeScope();
            builder.RegisterType<MainViewModel>();
            return builder;
        }
    }
}
