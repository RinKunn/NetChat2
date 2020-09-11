using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetChat2.Models;
using Api = NetChat2.FileMessaging;

namespace NetChat2.Services.Messaging
{
    public class NetChatMessageSender : IMessageSender
    {
        public void SendMessage(Chat chat, TextMessage message)
        {
            var path = chat.ChatData.SourcePath;
            var encoding = chat.ChatData.SourceEncoding;
            var apisender = new Api.MessageSender(path, encoding);
            apisender.SendMessage(message.Sender.Id, message.Text, message.Date);
        }

        public void SendUserStatusMessage(Chat chat, string userId, DateTime dateTime, UserStatus userStatus)
        {
            var path = chat.ChatData.SourcePath;
            var encoding = chat.ChatData.SourceEncoding;
            var apisender = new Api.MessageSender(path, encoding);
            apisender.SendUserStatusMessage(userId, userStatus == UserStatus.Online, dateTime);
        }
    }
}
