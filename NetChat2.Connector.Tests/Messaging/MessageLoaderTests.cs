using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NUnit.Framework;
using NetChat2.FileMessaging;

namespace NetChat2.FileMessaging.Tests.Messaging
{
    [TestFixture]
    class MessageLoaderTests
    {
        private string path;
        private Encoding encoding = Encoding.GetEncoding(1251);
        private DateTime baseDateTime;

        [OneTimeSetUp]
        public void Setup()
        {
            path = $@".\{Guid.NewGuid()}.txt";
            var sender = new MessageSender(path, encoding);
            baseDateTime = DateTime.Now;
            sender.SendUserStatusMessage("User1", true, baseDateTime.AddMinutes(-6));
            sender.SendMessage("User1", "Hello, my name us User1", baseDateTime.AddMinutes(-5));
            sender.SendUserStatusMessage("User3", true, baseDateTime.AddMinutes(-4));
            sender.SendMessage("User3", "Hello, my name us User3", baseDateTime.AddMinutes(-3));
            sender.SendUserStatusMessage("User3", false, baseDateTime.AddMinutes(-2));
            sender.SendUserStatusMessage("User2", true, baseDateTime.AddMinutes(-1));
            sender.SendMessage("User2", "Hello, my name us User2", baseDateTime);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            File.Delete(path);
        }

        [Test]
        public void LoadMessages_Return3Messages()
        {
            var loader = new MessageLoader(path, encoding);

            var messages = loader.LoadMessages();

            Assert.NotNull(messages);
            Assert.AreEqual(3, messages.Length);
            Assert.AreEqual("Hello, my name us User1", messages[0].Text);
            Assert.AreEqual("Hello, my name us User2", messages[2].Text);
            Assert.AreEqual(
                new DateTime(baseDateTime.Year, baseDateTime.Month, baseDateTime.Day,
                baseDateTime.Hour, baseDateTime.Minute, baseDateTime.Second), messages[2].DateTime);
        }

        [Test]
        public void Load3Messages_Return1Message()
        {
            var loader = new MessageLoader(path, encoding);

            var messages = loader.LoadMessages(3);

            Assert.NotNull(messages);
            Assert.AreEqual(1, messages.Length);
            Assert.AreEqual("Hello, my name us User2", messages[0].Text);
        }

        [Test]
        public void GetUsersIds_Return3Ids()
        {
            var loader = new MessageLoader(path, encoding);

            var idlist = loader.GetUsersIds();

            Assert.NotNull(idlist);
            Assert.AreEqual(3, idlist.Length);
            Assert.AreEqual("User1", idlist[0]);
            Assert.AreEqual("User2", idlist[2]);
        }

        [Test]
        public void GetUsersStatus_2Online1Offline()
        {
            var loader = new MessageLoader(path, encoding);

            var userStatuses = loader.GetUsersStatus();

            Assert.NotNull(userStatuses);
            Assert.AreEqual(3, userStatuses.Length);
            Assert.AreEqual("User2", userStatuses[0].UserId);
            Assert.IsTrue(userStatuses[0].IsOnline);
            Assert.AreEqual("User3", userStatuses[1].UserId);
            Assert.IsFalse(userStatuses[1].IsOnline);
            Assert.AreEqual("User1", userStatuses[2].UserId);
            Assert.IsTrue(userStatuses[2].IsOnline);
        }
    }
}
