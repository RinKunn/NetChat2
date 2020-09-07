using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetChat2.Connector;
using NetChat2.Models;
using NetChat2.Persistance;

namespace NetChat2.Services
{
    public class DefaultMessageSender : IMessageSender
    {
        private readonly IChatRepository _chatRepository;

        public DefaultMessageSender(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository ?? throw new ArgumentNullException(nameof(chatRepository));
        }

        public void SendMessage(int chatId, TextMessage message)
        {
            var chatData = _chatRepository.GetChatById(chatId);
            var sender = new MessageFileSender(chatData.SourcePath, Encoding.GetEncoding(chatData.SourceEncodingName));

            sender.SendMessage(new NetChatMessage(
                message.Sender.Id,
                message.Text,
                message.Date));
        }
    }
}
