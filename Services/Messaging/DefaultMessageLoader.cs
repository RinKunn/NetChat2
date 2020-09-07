using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetChat2.Connector;
using NetChat2.Models;
using NetChat2.Persistance;

namespace NetChat2.Services
{
    public class DefaultMessageLoader : IMessageLoader
    {
        private readonly IChatRepository _chatRepository;

        public DefaultMessageLoader(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository ?? throw new ArgumentNullException(nameof(chatRepository));
        }

        public List<TextMessage> LoadMessages(int chatId, int limit = 0)
        {
            var chatData = _chatRepository.GetChatById(chatId);
            var loader = new MessageFileLoader(chatData.SourcePath, Encoding.GetEncoding(chatData.SourceEncodingName));

            return loader.LoadMessages(limit)?
                .Where(netMessage => netMessage.Text != "Logon" && netMessage.Text != "Logout")
                .Select(netMessage =>
                    new TextMessage()
                    {
                        Date = netMessage.DateTime,
                        Text = netMessage.Text,
                        SenderId = netMessage.UserName
                    })
                .ToList();
        }
    }
}
