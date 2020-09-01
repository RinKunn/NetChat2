using NetChat2.Models;

namespace NetChat2.ViewModel.Messages
{
    public class MessageReceived : IMessage
    {
        public TextMessageViewModel TextMessageViewModel { get; private set; }

        public MessageReceived(TextMessageViewModel message) => TextMessageViewModel = message;
    }
}
