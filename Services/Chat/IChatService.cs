using System;
using System.Collections.Generic;
using System.Linq;
using NetChat2.Connector;
using NetChat2.Models;
using NetChat2.Persistance;

namespace NetChat2.Services
{
    public interface IChatService
    {
        User[] LoadUsersFromChat(int chatId);

        Chat GetChat(int chatId);

        void CreateChat(string title, string description = null);

        int GetOnlineUsersCount(int chatId);
    }
}
