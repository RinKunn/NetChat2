using System;
using System.IO;
using System.Text;
using NetChat2.Persistance;
using NUnit.Framework;

namespace NetChat2.Tests.Persistance
{
    [TestFixture]
    public class JsonChatRepositoryTests
    {
        private string chatsSource;
        private string currentUserId;
        
        private JsonChatRepository repository;

        [SetUp]
        public void BeforeGroupTesting()
        {
            chatsSource = $@".\chat_{Guid.NewGuid()}.json";
            currentUserId = Environment.UserName.ToUpper();
            repository = new JsonChatRepository(chatsSource);
        }

        [TearDown]
        public void AfterGroupTesting()
        {
            File.Delete(chatsSource);
        }

        [Test]
        public void CreateChat_ChatsSourceNotExists_FileCreated()
        {
            var res = repository.AddChat("title", currentUserId, $@".\{Guid.NewGuid()}.txt", Encoding.GetEncoding(1251), "desc");

            Assert.True(res);
            Assert.True(File.Exists(chatsSource));
            Assert.Greater((new FileInfo(chatsSource)).Length, 0);
        }

        [Test]
        public void CreateChatWithSamePath_ReturnFalse()
        {
            var res1 = repository.AddChat("title1", currentUserId, $@".\test.txt", Encoding.GetEncoding(1251), "desc1");
            var res2 = repository.AddChat("title2", currentUserId, $@".\test.txt", Encoding.GetEncoding(1251), "desc2");

            Assert.False(res2);
        }


        [Test]
        public void GetChatData_ReturnChatData()
        {
            var res1 = repository.AddChat("title1", currentUserId, $@".\{Guid.NewGuid()}.txt", Encoding.GetEncoding(1251), "desc1");
            var res2 = repository.AddChat("title2", currentUserId, $@".\{Guid.NewGuid()}.txt", Encoding.GetEncoding(1251), "desc2");

            var readedChatData = repository.GetChatById(1);

            Assert.IsTrue(res1);
            Assert.IsTrue(res2);
            Assert.AreEqual("title1", readedChatData.Title);
            Assert.AreEqual(Encoding.GetEncoding(1251), Encoding.GetEncoding(readedChatData.EncodingName));
        }

        [Test]
        public void GetNotExistsChatData_ReturnNull()
        {
            var res1 = repository.AddChat("title1", currentUserId, $@".\{Guid.NewGuid()}.txt", Encoding.GetEncoding(1251), "desc1");

            var readedChatData = repository.GetChatById(3);

            Assert.IsTrue(res1);
            Assert.IsNull(readedChatData);
        }
    }
}
