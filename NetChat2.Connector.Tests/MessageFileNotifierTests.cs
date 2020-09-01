using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using NUnit.Framework;

namespace NetChat2.Connector.Tests
{
    [TestFixture]
    class MessageFileNotifierTests
    {
        private string path;
        private Encoding encoding = Encoding.GetEncoding(1251);

        [SetUp]
        public void Init()
        {
            string dir = Path.Combine(Directory.GetCurrentDirectory(), "NetChatTests");
            path = Path.Combine(dir, "mfnt.txt");
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

            using(var watcher = new MessageFileNotifier(path, encoding))
            {
                watcher.OnMessageReceived += (mess) =>
                {
                    receivedMessages.Add(mess);
                    statsUpdatedEvent.Set();
                };

                var sender = new MessageFileSender(path, encoding);
                sender.SendMessage(new NetChatMessage(username, inputLine));

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

            using (var watcher = new MessageFileNotifier(path, encoding))
            {
                watcher.OnMessageReceived += (mess) =>
                {
                    receivedMessages.Add(mess);
                    statsUpdatedEvent.Set();
                };

                var sender = new MessageFileSender(path, encoding);
                sender.SendMessage(new NetChatMessage(username, inputLine));

                statsUpdatedEvent.WaitOne(50, false);
            }

            Assert.AreEqual(1, receivedMessages.Count);
            Assert.AreEqual(inputLine, receivedMessages[0].Text);
            Assert.AreEqual(username, receivedMessages[0].UserName);
        }
    }
}
