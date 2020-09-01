using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NetChat2;
using NetChat2.ViewModel;
using Autofac;
using System.Threading;

namespace NetChat2.Tests
{

    [TestFixture]
    public class MainViewModelTests
    {
        private MainViewModel viewmodel = null;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAppServices();
            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope())
                viewmodel = scope.Resolve<MainViewModel>();
        }

        [OneTimeTearDown]
        public void Cleanup()
        {

        }

        [Test]
        public void ResolvedNotNull()
        {
            Assert.NotNull(viewmodel);
        }

        [Test]
        public async Task Connect_IsConnectedTrue()
        {
            await viewmodel.ConnectCommand.ExecuteAsync(null);

            Assert.IsTrue(viewmodel.IsConnected);
            Assert.IsFalse(viewmodel.ConnectCommand.CanExecute(null));
        }

        [Test]
        public async Task Logout_IsConnectedFalse()
        {
            await viewmodel.ConnectCommand.ExecuteAsync(null);

            viewmodel.LogoutCommand.Execute(null);

            Assert.IsFalse(viewmodel.IsConnected);
            Assert.IsTrue(viewmodel.ConnectCommand.CanExecute(null));
        }

        [Test]
        public async Task SendMessage_MessageAddedToList()
        {
            await viewmodel.ConnectCommand.ExecuteAsync(null);
            //viewmodel.TextMessage = "message";

            viewmodel.SendMessageCommand.Execute(null);

            ManualResetEvent resetEvent = new ManualResetEvent(false);
            resetEvent.WaitOne(50, false);
            foreach(var mes in viewmodel.Messages)
                Console.WriteLine(mes.Text);
            Assert.AreEqual(2, viewmodel.Messages.Count);
            Assert.AreEqual("message", viewmodel.Messages[1].Text);
        }
    }
}
