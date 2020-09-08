using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace NetChat2.Persistance
{
    public class JsonChatRepository : IChatRepository
    {
        private readonly string _path;

        public JsonChatRepository(string path)
        {
            _path = string.IsNullOrWhiteSpace(path) ? throw new ArgumentNullException(path) : path;
        }

        public bool AddChat(string title, string authorId, string path, Encoding encoding, string description = null)
        {
            if (string.IsNullOrEmpty(title)) throw new ArgumentNullException(title);
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(path);
            if (string.IsNullOrEmpty(authorId)) throw new ArgumentNullException(authorId);

            List<StoredChatData> chats =
                File.Exists(_path)
                ? JsonConvert.DeserializeObject<List<StoredChatData>>(File.ReadAllText(_path))
                : new List<StoredChatData>();

            if (chats.SingleOrDefault(c => c.SourcePath == path) != null) return false;

            StoredChatData chatData = new StoredChatData()
            {
                Id = chats.Count > 0 ? chats.Max(c => c.Id) + 1 : 1,
                Title = title,
                AuthorId = authorId,
                Description = description,
                SourcePath = path,
                SourceEncodingName = encoding == null ? Encoding.UTF8.SourceEncodingName : encoding.HeaderName,
                Members = new List<string>() { authorId }
            };
            
            chats.Add(chatData);

            FileAttempts.TryWriteAllText(_path, JsonConvert.SerializeObject(chats));
            return true;
        }

        public StoredChatData GetChatById(int id)
        {
            if (!File.Exists(_path) || (new FileInfo(_path)).Length == 0) return null;

            return JsonConvert
                .DeserializeObject<List<StoredChatData>>(File.ReadAllText(_path))
                .SingleOrDefault(c => c.Id == id);
        }
    }
}
