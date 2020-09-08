using NetChat2.Models;
using System;

namespace NetChat2.ViewModel.Messages
{
    public class MessageReceived : IMessage
    {
        //public TextMessageViewModel TextMessageViewModel { get; private set; }

        //public MessageReceived(DateTime dateTime, User author, string text, User currentUser)
        //{
        //    if (author == null) throw new ArgumentNullException(nameof(author));
        //    if (currentUser == null) throw new ArgumentNullException(nameof(currentUser));

        //    TextMessageViewModel = new TextMessageViewModel(
        //        dateTime, author, text,
        //        author.Id == currentUser.Id,
        //        author.Id == currentUser.Id);
        //}

        public TextMessage Message { get; private set; }
        
        public MessageReceived(TextMessage message)
        {
            Message = message;
        }
    }
}
