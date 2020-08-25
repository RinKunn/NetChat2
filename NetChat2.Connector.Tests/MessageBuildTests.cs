using System;
using NUnit.Framework;

namespace NetChat2.Connector.Tests
{
    [TestFixture]
    public class MessageBuildTests
    {
        [Test]
        public void CleanLine()
        {
            string line = "24.08 17:00:47|FGHFFGH> Hello! My name is '5cc62'";
            NetChatMessage mes = new NetChatMessage(line);

            Assert.AreEqual(new DateTime(2020, 08, 24, 17, 0, 47), mes.DateTime);
            Assert.AreEqual("FGHFFGH", mes.UserName);
            Assert.AreEqual("Hello! My name is '5cc62'", mes.Text);
        }

        [Test]
        public void NameWithWhitespaces()
        {
            string line = "24.08 17:00:47|FGHFF GH dfdf> Hello! My name is '5cc62'";
            NetChatMessage mes = new NetChatMessage(line);

            Assert.AreEqual(new DateTime(2020, 08, 24, 17, 0, 47), mes.DateTime);
            Assert.AreEqual("FGHFF GH dfdf", mes.UserName);
            Assert.AreEqual("Hello! My name is '5cc62'", mes.Text);
        }

        [Test]
        public void MessageWithGreaterThan()
        {
            string line = "24.08 17:00:47|FGHFF GH dfdf> Hello! My name > is '5cc > 62'";
            NetChatMessage mes = new NetChatMessage(line);

            Assert.AreEqual(new DateTime(2020, 08, 24, 17, 0, 47), mes.DateTime);
            Assert.AreEqual("FGHFF GH dfdf", mes.UserName);
            Assert.AreEqual("Hello! My name > is '5cc > 62'", mes.Text);
        }


    }
}
