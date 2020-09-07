using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat2.Persistance
{
    public class ChatNotExistsExecption : Exception
    {
        public int ChatId { get; private set; }

        public ChatNotExistsExecption(int chatId) 
            : base($"User id='{chatId}' not exists")
        {
            ChatId = chatId;
        }
    }
}
