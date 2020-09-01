using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NetChat2.Services.Persistance
{
    public class JsonChatRepository : IChatRepository
    {
        private readonly string _path;

        public JsonChatRepository(string path)
        {
            _path = string.IsNullOrWhiteSpace(path) ? throw new ArgumentNullException(path) : path;
        }

        public bool CreateChat(ChatData chat)
        {
            List<ChatData> chats = 
                File.Exists(_path)
                ? JsonConvert.DeserializeObject<List<ChatData>>(File.ReadAllText(_path))
                : new List<ChatData>();

            if (chats.SingleOrDefault(c => c.ChatPath == chat.ChatPath) != null) return false;

            chat.Id = chats.Max(c => c.Id) + 1;
            chats.Add(chat);
            File.WriteAllText(_path, JsonConvert.SerializeObject(chats));
            return true;
        }

        public ChatData GetChatData(int id)
        {
            if (!File.Exists(_path)) return null;
            return JsonConvert
                .DeserializeObject<List<ChatData>>(File.ReadAllText(_path))
                .SingleOrDefault(c => c.Id == id);
        }
    }
}
