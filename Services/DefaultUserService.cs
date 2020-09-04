using System;
using System.Collections.Generic;
using System.Diagnostics;
using NetChat2.Models;
using NetChat2.Persistance;

namespace NetChat2.Services
{
    public class DefaultUserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMessageService _messageService;

        public DefaultUserService(IUserRepository userRepository, IMessageService messageService)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
        }

        public User GetMe()
        {
            string envUserName = GetMyUserId();
            var userData = _userRepository.GetUserById(envUserName);
            if (userData == null)
                return CreateDefaultUser(envUserName);
            return UserDataToUser(userData);
        }

        private string GetMyUserId()
        {
            string envUserName = Environment.UserName.ToUpper();
#if DEBUG
            var processesCount = Process.GetProcessesByName("NetChat2").Length;
            if (processesCount > 1) envUserName += processesCount;
#endif
            return envUserName;
        }

        public bool IsMe(string envUserName)
        {
            return envUserName == GetMyUserId();
        }


        public void Logon()
        {
            string id = GetMyUserId();
            DateTime logonDate = DateTime.Now;
            var user = _userRepository.GetUserById(id);
            if (user == null)
            {
                // разделение модели и сервисов

                user = new StoredUserData()
                {
                    Id = id,
                    Surname = id,
                    Name = "",
                    Lastname = "",
                    Status = (int)UserStatus.Online,
                    StatusLastChanged = logonDate,
                    ChatsIds = new List<int>() { 1 }
                };,
            }
            else
            {
                user.Status = (int)UserStatus.Online;
                user.StatusLastChanged = DateTime.Now;
            }
            foreach(var chatId in user.ChatsIds)
            {
                _messageService.SendMessage(chatId, new TextMessage()
                {
                    Date = logonDate,
                    SenderId = user.Id,
                    ChatId = chatId,
                    Text = "Logon"
                });
            }
            _userRepository.ChangeStatus(id, UserStatus.Online);
        }

        public void Logout()
        {
            _userRepository.ChangeStatus(GetMyUserId(), UserStatus.Offline);
        }

        public User GetUser(string envUserName)
        {
            var userData = _userRepository.GetUserById(envUserName);
            if (userData == null)
                return null;

            return UserDataToUser(userData);
        }

        private User UserDataToUser(StoredUserData userData)
        {
            return new User()
            {
                EnvName = userData.EnvName,
                Surname = userData.Surname,
                Name = userData.Name,
                Lastname = userData.Lastname,
                Self = userData.EnvName == Environment.UserName.ToUpper(),
                Status = (UserStatus)userData.Status,
                StatusChangedDateTime = userData.StatusLastChanged
            };
        }

        private User CreateDefaultUser(string envUserName)
        {
            return new User()
            {
                EnvName = envUserName,
                Surname = envUserName,
                Name = envUserName,
                Lastname = envUserName,
                Self = envUserName == Environment.UserName.ToUpper(),
                Status = UserStatus.Online,
                StatusChangedDateTime = DateTime.Now
            };
        }
    }
}
