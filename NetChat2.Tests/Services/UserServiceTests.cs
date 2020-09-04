using System;
using System.IO;
using Autofac;
using NetChat2.Models;
using NetChat2.Persistance;
using NetChat2.Services;
using NUnit.Framework;

namespace NetChat2.Tests.Services
{
    [TestFixture]
    public class UserServiceTests
    {

        private IUserService userService;
        private string path = @".\usersNetchat2.dat";

        [SetUp]
        public void SetUp()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<DefaultUserService>().As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<JsonUserRepository>()
                .As<IUserRepository>()
                .WithParameter(new TypedParameter(typeof(string), path))
                .InstancePerLifetimeScope();
            var cont = builder.Build();
            using (var scope = cont.BeginLifetimeScope())
                userService = scope.Resolve<IUserService>();
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            File.Delete(path);
        }

        [Test]
        public void GetMe_UserSourceNotExists()
        {
            var user = userService.GetMe();

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
