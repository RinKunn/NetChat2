using System;
using System.IO;
using System.Linq;
using System.Text;
using NetChat2.Connector;
using NetChat2.Models;
using NetChat2.Persistance;

namespace NetChat2.Services
{
    public class DefaultChatLoader : IChatService
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

        public User[] LoadUsersFromChat(int chatId)
        {
            return LoadMessages(chatId)?
                .Select(m => m.UserName)
                .Distinct()
                .Select(envName => _userService.GetUser(envName))
                .ToArray();
        }

        public Chat GetChat(int chatId)
        {
            var me = _userService.GetMe();
            var chatData = _chatRepository.GetChatById(chatId);
            if (chatData == null) return null;

            return new Chat()
            {
                ChatData = new ChatData()
                {
                   Id = chatData.Id,
                   Title = chatData.Title,
                   Description = chatData.Description
                },
                User = me,
                UnreadCount = 0
            };
        }

        public void CreateChat(string title, string description = null)
        {
            int id = _chatRepository.ChatsCount() + 1;
            string path = Path.Combine(Properties.Settings.Default.ChatsDir, $"chat_{id}.dat");
            _chatRepository.AddChat(title, "", path, Encoding.GetEncoding(1251), description);
        }

        public int GetOnlineUsersCount(int chatId)
        {
            return LoadUsersFromChat(chatId)?
                .Where(u => u.Status == UserStatus.Online)?.Count() ?? 0;
        }



        private NetChatMessage[] LoadMessages(int chatId)
        {
            var chatData = _chatRepository.GetChatById(chatId);
            if (chatData == null) return null;

            var reader = new MessageFileLoader(chatData.SourcePath, Encoding.GetEncoding(chatData.SourceEncodingName));
            return reader.LoadMessages();
        }
    }
}
