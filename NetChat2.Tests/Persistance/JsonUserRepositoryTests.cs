using System;
using System.IO;
using NetChat2.Models;
using NetChat2.Persistance;
using NUnit.Framework;

namespace NetChat2.Tests.Persistance
{
    [TestFixture]
    public class JsonUserRepositoryTests
    {
        private readonly string envUserName = Environment.UserName;
        private string path;
        private JsonUserRepository repository;

        [SetUp]
        public void BeforeGroupTesting()
        {
            path = $@".\{Guid.NewGuid()}.json";
            repository = new JsonUserRepository(path);
        }

        [TearDown]
        public void AfterGroupTesting()
        {
            File.Delete(path);
        }


        [Test]
        public void Logon_Emptyid_ThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => repository.ChangeStatus(null, UserStatus.Online));
            Assert.Throws<ArgumentNullException>(() => repository.ChangeStatus(string.Empty, UserStatus.Online));
        }

        [Test]
        public void Logon_FileNotExists_ThrowException()
        {
            Assert.Throws<UserNotExistsExecption>(() => repository.ChangeStatus(envUserName, UserStatus.Online));
            Assert.False(File.Exists(path));
        }

        [Test]
        public void Logon_OfflineUser_StatusChanged()
        {
            repository.Add(envUserName, envUserName, null, null, UserStatus.Offline, DateTime.Now.AddDays(1));

            repository.ChangeStatus(envUserName, UserStatus.Online);

            var userData = repository.GetUserById(envUserName);
            Assert.True(File.Exists(path));
            Assert.Greater((new FileInfo(path)).Length, 0);
            Assert.AreEqual((int)UserStatus.Online, userData.Status);
            Assert.AreEqual(DateTime.Today.Day, userData.StatusLastChanged.Date.Day);
        }

        [Test]
        public void GetAll_Return1UserData()
        {
            var users = repository.GetAll();

            Assert.AreEqual(1, users.Length);
            Assert.AreEqual(envUserName, users[0].Id);
            Assert.AreEqual((int)UserStatus.Online, users[0].Status);
        }

        [Test]
        public void GetAll_FileNotExists_ReturnNull()
        {
            File.Delete(path);

            var users = repository.GetAll();

            Assert.IsNull(users);
        }

        [Test]
        public void GetExistsUser_ReturnUserDate()
        {
            repository.Add(envUserName, envUserName, null, null, UserStatus.Online, DateTime.Now.AddDays(1));
            repository.Add(envUserName + 2, envUserName + 2, null, null, UserStatus.Online, DateTime.Now.AddDays(1));
            repository.Add(envUserName, envUserName, null, null, UserStatus.Offline, DateTime.Now.AddDays(1));

            var userData = repository.GetUserById(envUserName);

            Assert.AreEqual(envUserName, userData.Id);
            Assert.AreEqual((int)UserStatus.Offline, userData.Status);
        }

        [Test]
        public void GetNotExistsUser_ReturnNull()
        {
            repository.Add(envUserName, envUserName, null, null, UserStatus.Online, DateTime.Now.AddDays(1));

            var userData = repository.GetUserById(envUserName + "2");

            Assert.IsNull(userData);
        }

        [Test]
        public void IncludeUserToChat()
        {
            repository.Add(envUserName, envUserName, null, null, UserStatus.Online, DateTime.Now.AddDays(1));

            repository.IncludeToChat(envUserName, 1);

            var userData = repository.GetUserById(envUserName);
            Assert.AreEqual(1, userData.ChatsIds.Count);
            Assert.AreEqual(1, userData.ChatsIds[0]);
        }

        [Test]
        public void IncludeNotExistsUserToChat()
        {
            repository.Add(envUserName, envUserName, null, null, UserStatus.Online, DateTime.Now.AddDays(1));

            Assert.Throws<UserNotExistsExecption>(() => repository.IncludeToChat(envUserName + 2, 1));
        }

        [Test]
        public void OnlineUsersCount_Return1()
        {
            repository.Add(envUserName, envUserName, null, null, UserStatus.Online, DateTime.Now.AddDays(3));
            repository.Add(envUserName, envUserName, null, null, UserStatus.Offline, DateTime.Now);
            repository.Add(envUserName + 2, envUserName + 2, null, null, UserStatus.Offline, DateTime.Now.AddDays(1));

            var count = repository.OnlineUsersCount(new string[] { envUserName + 2, envUserName });

            Assert.AreEqual(1, count);
        }
    }
}
