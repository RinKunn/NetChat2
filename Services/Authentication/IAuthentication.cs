using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetChat2.Models;
using NetChat2.Services;
using NetChat2.Persistance;
using NLog;

namespace NetChat2.Services
{
    public interface IAuthentication
    {
        void Login(string userId);
        void Logout(string userId);
    }

    public class Authentication : IAuthentication
    {
        //private static Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly IMessageSender _messageSender;
        private readonly IUserRepository _userRepository;

        public Authentication(IUserRepository userRepository, IMessageSender messageSender)
        {
            _userRepository = userRepository;
            _messageSender = messageSender;
        }

        public void Login(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId), "Logging in userId must not be empty");

            DateTime logonDate = DateTime.Now;
            var user = _userRepository.GetUserById(userId);
            if (user == null)
            {
                user = new StoredUserData()
                {
                    Id = userId,
                    Surname = userId,
                    Name = "",
                    Lastname = "",
                    Status = (int)UserStatus.Online,
                    StatusLastChanged = logonDate,
                    ChatsIds = new List<int>() { 1 }
                };
            }
            else
            {
                user.Status = (int)UserStatus.Online;
                user.StatusLastChanged = DateTime.Now;
            }
            if (!_userRepository.Update(user))
                throw new AuthenticationException($"Cannot set logon status to '{userId}'");

            foreach (var chatId in user.ChatsIds)
            {
                _messageSender.SendMessage(chatId, new TextMessage()
                {
                    Date = logonDate,
                    Sender = user,
                    ChatId = chatId,
                    Text = "Logon"
                });
            }
        }

        public void Logout(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId), "Logging in userId must not be empty");

            DateTime logoutTime = DateTime.Now;
            var user = _userRepository.GetUserById(userId);

            if (user == null)
                throw new AuthenticationException($"Logging out user '{userId}' cannot find!");

            user.Status = (int)UserStatus.Online;
            user.StatusLastChanged = DateTime.Now;

            if (!_userRepository.Update(user))
                throw new AuthenticationException($"Cannot set logout status to '{userId}'");

            foreach (var chatId in user.ChatsIds)
            {
                _messageService.SendMessage(chatId, new TextMessage()
                {
                    Date = logoutTime,
                    SenderId = user.Id,
                    ChatId = chatId,
                    Text = "Logout"
                });
            }
        }
    }
}
