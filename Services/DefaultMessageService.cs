using System.Collections.Generic;
using System.Linq;
using NetChat2.Connector;
using NetChat2.Models;
using NetChat2.Services.Persistance;

namespace NetChat2.Services
{
    public class DefaultMessageService : IMessageService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IUserService _userService;

        public DefaultMessageService(IChatRepository chatRepository, IUserService userService)
        {
            _chatRepository = chatRepository;
            _userService = userService;
        }

        public List<TextMessage> LoadMessages(int chatId, int limit = 0)
        {
            var chatData = _chatRepository.GetChatData(chatId);
            var loader = new MessageFileLoader(chatData.ChatPath, chatData.Encoding);

            return loader.LoadMessages(limit)?
                .Where(netMessage => netMessage.Text != "Logon" && netMessage.Text != "Logout")
                .Select(netMessage =>
                    new TextMessage()
                    {
                        CreatedDateTime = netMessage.DateTime,
                        MessageText = netMessage.Text,
                        Author = _userService.GetUser(netMessage.UserName)
                    })
                .ToList();
        }

        public void SendMessage(Chat chat, TextMessage message)
        {
            var chatData = _chatRepository.GetChatData(chat.Id);
            var sender = new MessageFileSender(chatData.ChatPath, chatData.Encoding);

            sender.SendMessage(new NetChatMessage(
                message.Author.EnvName, message.MessageText, message.CreatedDateTime));
        }
    }
}
