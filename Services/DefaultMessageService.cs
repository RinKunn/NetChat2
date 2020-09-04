using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetChat2.Connector;
using NetChat2.Models;
using NetChat2.Persistance;

namespace NetChat2.Services
{
    public class DefaultMessageService : IMessageService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IUserService _userService;

        public DefaultMessageService(IChatRepository chatRepository, IUserService userService)
        {
            _chatRepository = chatRepository ?? throw new ArgumentNullException(nameof(chatRepository));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public List<TextMessage> LoadMessages(int chatId, int limit = 0)
        {
            var chatData = _chatRepository.GetChatById(chatId);
            var loader = new MessageFileLoader(chatData.ChatPath, Encoding.GetEncoding(chatData.EncodingName));

            return loader.LoadMessages(limit)?
                .Where(netMessage => netMessage.Text != "Logon" && netMessage.Text != "Logout")
                .Select(netMessage =>
                    new TextMessage()
                    {
                        Date = netMessage.DateTime,
                        MessageText = netMessage.Text,
                        Sender = _userService.GetUser(netMessage.UserName)
                    })
                .ToList();
        }

        public void SendMessage(int chatId, TextMessage message)
        {
            var chatData = _chatRepository.GetChatById(chatId);
            var sender = new MessageFileSender(chatData.ChatPath, Encoding.GetEncoding(chatData.EncodingName));

            sender.SendMessage(new NetChatMessage(
                message.Sender.EnvName, message.MessageText, message.Date));
        }
    }
}
