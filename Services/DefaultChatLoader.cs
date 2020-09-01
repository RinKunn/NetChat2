using System;
using System.Linq;
using NetChat2.Connector;
using NetChat2.Models;
using NetChat2.Services.Persistance;

namespace NetChat2.Services
{
    public class DefaultChatLoader : IChatLoader
    {
        private readonly IChatRepository _chatRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        
        public DefaultChatLoader(
            IChatRepository chatRepository,
            IUserRepository userRepository,
            IUserService userService)
        {
            _chatRepository = chatRepository;
            _userRepository = userRepository;
            _userService = userService;
        }

        public User[] LoadChatUsers(Chat chat)
        {
            var chatData = _chatRepository.GetChatData(chat.Id);
            if (chatData == null) return null;

            string meEnvName = Environment.UserName.ToUpper();

            var reader = new MessageFileLoader(chatData.ChatPath, chatData.Encoding);

            return reader.LoadMessages()?
                .Select(m => m.UserName)
                .Distinct()
                .Select(envName =>
                {
                    var userData = _userRepository.GetUserByEnvName(envName);
                    return new User()
                    {
                        //TODO: use mappers
                        EnvName = userData.EnvName,
                        Surname = userData.Surname,
                        Name = userData.Name,
                        Lastname = userData.Lastname,
                        Self = userData.EnvName == meEnvName,
                        Status = (UserStatus)userData.Status,
                        StatusChangedDateTime = userData.StatusLastChanged
                    };
                })
                .ToArray();
        }

        public Chat LoadChat(int chatId)
        {
            ChatData chatData = _chatRepository.GetChatData(chatId);
            if (chatData == null) return null;
            return new Chat(chatData.Id, chatData.Title, chatData.Description);
        }
    }
}
