using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetChat2.Models;

namespace NetChat2.ViewModel.InnerMessages
{
    internal class MessageReceived : IInnerMessage
    {
        public TextMessage Message { get; private set; }

        public MessageReceived(TextMessage message)
        {
            Message = message;
        }
    }
}
