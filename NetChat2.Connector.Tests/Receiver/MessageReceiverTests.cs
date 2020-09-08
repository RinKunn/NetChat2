using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NetChat2.Api.Tests.Receiver
{
    [TestFixture]
    public class MessageReceiverTests
    {
        private string path;
        private Encoding encoding = Encoding.GetEncoding(1251);
        private MessageSender sender;

        [OneTimeSetUp]
        public void Init()
        {
            path = $@".\{Guid.NewGuid()}.txt";
            sender = new MessageSender(path, encoding);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            File.Delete(path);
        }


        [Test]
        public void OnMessageReceivedRaised()
        {
            string username = Guid.NewGuid().ToString().Substring(0, 5);
            string inputLine = $"Hello, my name is {username}";
            List<NetChatMessage> receivedMessages = new List<NetChatMessage>();
            ManualResetEvent statsUpdatedEvent = new ManualResetEvent(false);

            using (var watcher = new MessageReceiver(path, encoding))
            {
                watcher.OnMessageReceived += (mess) =>
                {
                    receivedMessages.Add(mess);
                    statsUpdatedEvent.Set();
                };

                sender.SendMessage(username, inputLine, DateTime.Now);
                statsUpdatedEvent.WaitOne(100, false);
            }

            Assert.AreEqual(1, receivedMessages.Count);
            Assert.AreEqual(inputLine, receivedMessages[0].Text);
            Assert.AreEqual(username, receivedMessages[0].UserName);
        }

        [Test]
        public void SendMessageAfterDispose_EventNotRaised()
        {
            string username = Guid.NewGuid().ToString().Substring(0, 5);
            string inputLine = $"Hello, my name is {username}";
            List<NetChatMessage> receivedMessages = new List<NetChatMessage>();
            ManualResetEvent statsUpdatedEvent = new ManualResetEvent(false);

            using (var watcher = new MessageReceiver(path, encoding))
            {
                watcher.OnMessageReceived += (mess) =>
                {
                    receivedMessages.Add(mess);
                    statsUpdatedEvent.Set();
                };   
            }

            sender.SendMessage(username, inputLine, DateTime.Now);
            statsUpdatedEvent.WaitOne(500, false);

            Assert.AreEqual(0, receivedMessages.Count);
        }

        [Test]
        public void OnUserLoggedInRaised()
        {
            var receivedMessages = new List<OnUserStatusChangedArgs>();
            ManualResetEvent statsUpdatedEvent = new ManualResetEvent(false);

            DateTime loggedInTime = new DateTime(2020, 01, 01, 10, 13, 44);
            using (var watcher = new MessageReceiver(path, encoding))
            {
                watcher.OnUserLoggedIn += (mess) =>
                {
                    receivedMessages.Add(mess);
                    statsUpdatedEvent.Set();
                };

                sender.SendUserStatusMessage("User1", true, loggedInTime);
                statsUpdatedEvent.WaitOne(100, false);
            }

            Assert.AreEqual(1, receivedMessages.Count);
            Assert.AreEqual(loggedInTime, receivedMessages[0].DateTime);
            Assert.AreEqual("User1", receivedMessages[0].UserId);
        }

        [Test]
        public void OnUserLoggedOutRaised()
        {
            var receivedMessages = new List<OnUserStatusChangedArgs>();
            ManualResetEvent statsUpdatedEvent = new ManualResetEvent(false);

            DateTime loggedInTime = new DateTime(2020, 01, 01, 10, 13, 44);
            using (var watcher = new MessageReceiver(path, encoding))
            {
                watcher.OnUserLoggedOut += (mess) =>
                {
                    receivedMessages.Add(mess);
                    statsUpdatedEvent.Set();
                };

                sender.SendUserStatusMessage("User1", false, loggedInTime);
                statsUpdatedEvent.WaitOne(100, false);
            }

            Assert.AreEqual(1, receivedMessages.Count);
            Assert.AreEqual(loggedInTime, receivedMessages[0].DateTime);
            Assert.AreEqual("User1", receivedMessages[0].UserId);
        }
    }
}
