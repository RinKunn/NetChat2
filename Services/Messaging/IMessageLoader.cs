using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetChat2.Models;

namespace NetChat2.Services
{
    public interface IMessageLoader
    {
        List<TextMessage> LoadMessages(Chat chat, int limit = 0);
        Task<List<TextMessage>> LoadMessagesAsync(Chat chat, int limit = 0);
    }
}
