using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetChat2.Models;
using NetChat2.Services;
using NetChat2.Persistance;

namespace NetChat2.Services
{
    public interface IAuthentication
    {
        void Login();
        void Logout();
    }

    public class Authentication : IAuthentication
    {
        private readonly IMessageService _messageService;
        private readonly IUserRepository _userRepository;

        public Authentication(IUserRepository userRepository, IMessageService messageService)
        {
            _userRepository = userRepository;
            _messageService = messageService;
        }


        public void Login()
        {
            string id = _userRepository.get();
            DateTime logonDate = DateTime.Now;
            var user = _userRepository.GetUserById(id);
            if (user == null)
            {
                user = new StoredUserData()
                {
                    Id = id,
                    Surname = id,
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
            foreach (var chatId in user.ChatsIds)
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
            
        }
    }
}
