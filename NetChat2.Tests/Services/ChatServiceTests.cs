using System;
using System.IO;
using Autofac;
using NetChat2.Models;
using NetChat2.Services;
using NetChat2.Persistance;
using NUnit.Framework;

namespace NetChat2.Tests.Services
{
    [TestFixture]
    public class ChatServiceTests
    {
        private IChatLoader chatLoader;
        private IUserService userService;
        private string chatsPath = @".\chatsNetchat2.dat";
        private string usersPath = @".\usersNetchat2.dat";

        [SetUp]
        public void SetUp()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAppServices(chatsPath, usersPath);
            var cont = builder.Build();
            using (var scope = cont.BeginLifetimeScope())
            {
                chatLoader = scope.Resolve<IChatLoader>();
                userService = scope.Resolve<IUserService>();
            }
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            File.Delete(chatsPath);
            File.Delete(usersPath);
        }

        [Test]
        public void GetMe_UserSourceNotExists()
        {
            userService.Logon();

            var user = chatLoader.LoadChat(1);

            Assert.AreEqual(Environment.UserName.ToUpper(), user.EnvName);
            Assert.AreEqual(UserStatus.Online, user.Status);
        }

        [Test]
        public void Logon_CurrentUserLogon()
        {
            userService.Logon();

            var user = userService.GetUser(Environment.UserName.ToUpper());
            Assert.IsTrue(File.Exists(path));
            Assert.NotNull(user);
            Assert.AreEqual(Environment.UserName.ToUpper(), user.EnvName);
            Assert.AreEqual(UserStatus.Online, user.Status);
        }

        [Test]
        public void Logout_CurrentUserLogout()
        {
            userService.Logout();

            var user = userService.GetUser(Environment.UserName.ToUpper());
            Assert.IsTrue(File.Exists(path));
            Assert.NotNull(user);
            Assert.AreEqual(Environment.UserName.ToUpper(), user.EnvName);
            Assert.AreEqual(UserStatus.Offline, user.Status);
        }

    }
}
