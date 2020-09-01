using System;
using System.Linq;
using NetChat2.Connector;
using NetChat2.Models;
using NetChat2.Services.Persistance;

namespace NetChat2.Services
{
    public interface IChatLoader
    {
        User[] LoadChatUsers(Chat chat);
        Chat LoadChat(int chatId);
    }
}
