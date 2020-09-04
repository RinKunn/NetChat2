using System;
using System.Linq;
using NetChat2.Connector;
using NetChat2.Models;
using NetChat2.Persistance;

namespace NetChat2.Services
{
    public interface IChatLoader
    {
        User[] LoadChatUsers(int chatId);
        Chat LoadChat(int chatId);
    }
}
