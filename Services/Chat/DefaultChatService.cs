using System;
using System.IO;
using System.Linq;
using System.Text;
using Api = NetChat2.FileMessaging;
using NetChat2.Models;
using NetChat2.Persistance;

namespace NetChat2.Services
{
    public class DefaultChatService : IChatService
    {
        public int GetOnlineUsersCount(Chat chat)
        {
            var reader = new Api.MessageLoader(chat.ChatData.SourcePath, chat.ChatData.SourceEncoding);
            return reader.GetUsersStatus().Count(u => u.IsOnline);
        }
    }
}
