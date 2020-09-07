using System;
using System.Linq;
using System.Text;
using NetChat2.Connector;
using NetChat2.Models;
using NetChat2.Persistance;

namespace NetChat2.Services
{
    public class DefaultChatLoader : IChatLoader
    {
        private readonly IChatRepository _chatRepository;
        private readonly IUserService _userService;
        
        public DefaultChatLoader(
            IChatRepository chatRepository,
            IUserService userService)
        {
            _chatRepository = chatRepository ?? throw new ArgumentNullException(nameof(chatRepository));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public User[] LoadChatUsers(int chatId)
        {
            var chatData = _chatRepository.GetChatById(chatId);
            if (chatData == null) return null;

            var reader = new MessageFileLoader(chatData.SourcePath, Encoding.GetEncoding(chatData.SourceEncodingName));

            return reader.LoadMessages()?
                .Select(m => m.UserName)
                .Distinct()
                .Select(envName => _userService.GetUser(envName))
                .ToArray();
        }

        public ChatData LoadChat(int chatId)
        {
            var me = _userService.GetMe();
            StoredChatData chatData = _chatRepository.GetChatById(chatId);
            if (chatData == null) return null;
            return new ChatData()
            {
                Id = chatData.Id,
                Title = chatData.Title,
                Description = chatData.Description,
                User = me
            };
        }
    }
}
