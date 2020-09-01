using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetChat2.Models;

namespace NetChat2.Services
{
    public interface IMessageService
    {
        void SendMessage(Chat chat, TextMessage message);
        List<TextMessage> LoadMessages(int chatId, int limit = 0);
    }
}
