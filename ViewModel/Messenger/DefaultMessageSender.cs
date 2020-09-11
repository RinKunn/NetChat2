using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Api = NetChat2.FileMessaging;
using NetChat2.Models;
using NetChat2.Persistance;
using NetChat2.Services;

namespace NetChat2.ViewModel.Messenger
{
    public class DefaultMessageSender : IMessageSender
    {
        public void SendMessage(Chat chat, TextMessage message)
        {
            var sender = new Api.MessageSender(chat.ChatData.SourcePath, chat.ChatData.SourceEncoding);

            sender.SendMessage(message.Sender.Id,
                message.Text,
                message.Date);
        }

        public void SendUserStatusMessage(Chat chat, string userId, DateTime dateTime, UserStatus userStatus)
        {
            var sender = new Api.MessageSender(chat.ChatData.SourcePath, chat.ChatData.SourceEncoding);
            sender.SendUserStatusMessage(userId, userStatus == UserStatus.Online, dateTime);
        }
    }
}
