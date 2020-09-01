using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetChat2.Models;

namespace NetChat2.ViewModel.Messages
{
    public class ShowViewMessage : IMessage
    {
        public Chat Chat { get; private set; }

        //public ShowViewMessage(Chat chat) => Chat = chat;
    }
}
