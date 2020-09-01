using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetChat2.Models;

namespace NetChat2.Services
{

    public delegate void MessageReceivedHandler(Message message);

    public interface IMessageLoader
    {
        List<Message> LoadMessages(int chatId, int limit);

        event MessageReceivedHandler OnMessageReceived;
    }

    public class MessageLoader : IMessageLoader
    {
        public event MessageReceivedHandler OnMessageReceived;

        public List<Message> LoadMessages(int chatId, int limit)
        {
            throw new NotImplementedException();
        }
    }
}
