using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat2.Persistance
{
    public interface IChatRepository
    {
        StoredChatData GetChatById(int id);
        bool AddChat(string title, string authorId, string path, Encoding encoding, string description = null);
    }
}
