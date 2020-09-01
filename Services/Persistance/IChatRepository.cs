using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat2.Services.Persistance
{
    public interface IChatRepository
    {
        ChatData GetChatData(int id);
        bool CreateChat(ChatData chat);
    }

}
