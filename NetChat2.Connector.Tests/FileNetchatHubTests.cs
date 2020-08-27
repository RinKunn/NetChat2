using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using NUnit.Framework;

namespace NetChat2.Connector.Tests
{
    [TestFixture]
    class FileNetchatHubTests
    {
        private string path;

        [SetUp]
        public void Init()
        {
            string dir = Path.Combine(Directory.GetCurrentDirectory(), "NetChatTests");
            path = Path.Combine(dir, "fnchh_test.txt");
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        }

        [TearDown]
        public void Cleanup()
        {
            File.Delete(path);
            Directory.Delete(Path.GetDirectoryName(path));
        }

        [Test]
        public void OnMessageReceivedRaised()
        {
            string username = Guid.NewGuid().ToString().Substring(0, 5);
            string inputLine = $"Hello, my name is {username}";
            List<NetChatMessage> receivedMessages = new List<NetChatMessage>();
            ManualResetEvent statsUpdatedEvent = new ManualResetEvent(false);

            using(var watcher = new FileNetchatHub(path))
            {
                watcher.OnMessageReceived += (mess) =>
                {
                    receivedMessages.Add(mess);
                    statsUpdatedEvent.Set();
                };

                File.AppendAllText(path, (new NetChatMessage(username, inputLine)).ToString() + "\n");
                statsUpdatedEvent.WaitOne(50, false);
            }

            Assert.AreEqual(1, receivedMessages.Count);
            Assert.AreEqual(inputLine, receivedMessages[0].Text);
            Assert.AreEqual(username, receivedMessages[0].UserName);
        }

        [Test]
        public void OnMessageReceivedRaised_RUS()
        {
            string username = Guid.NewGuid().ToString().Substring(0, 5);
            string inputLine = $"Привет, меня зовут {username}";
            List<NetChatMessage> receivedMessages = new List<NetChatMessage>();
            ManualResetEvent statsUpdatedEvent = new ManualResetEvent(false);

            using (var watcher = new FileNetchatHub(path))
            {
                watcher.OnMessageReceived += (mess) =>
                {
                    receivedMessages.Add(mess);
                    statsUpdatedEvent.Set();
                };

                File.AppendAllText(path, (new NetChatMessage(username, inputLine)).ToString() + "\n");
                statsUpdatedEvent.WaitOne(50, false);
            }

            Assert.AreEqual(1, receivedMessages.Count);
            Assert.AreEqual(inputLine, receivedMessages[0].Text);
            Assert.AreEqual(username, receivedMessages[0].UserName);
        }
    }
}
