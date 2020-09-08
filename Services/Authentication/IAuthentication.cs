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
        void Login();
        void Logout();
    }

    public class Authentication : IAuthentication
    {
        //private static Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly IMessageSender _messageSender;
        private readonly IUserService _userService;

        public Authentication(IMessageSender messageSender, IUserService userService)
        {
            _messageSender = messageSender;
            _userService = userService;
        }

        public void Login()
        {
            DateTime logonDate = DateTime.Now;
            var user = _userService.GetMe();
            if (user.ChatIds.Count == 0) user.ChatIds.Add(1);
            
            if (!_userService.CreateOrUpdate(user))
                throw new AuthenticationException($"Cannot set logon status to '{user.Id}'");

            foreach (var chatId in user.ChatIds)
            {
                _messageSender.SendUserStatusMessage(chatId, user.Id, logonDate, UserStatus.Online);
            }
        }

        public void Logout()
        {
            DateTime logoutTime = DateTime.Now;
            var user = _userService.GetMe();
            user.Status = UserStatus.Offline;
            user.StatusChangedDateTime = logoutTime;

            if (!_userService.CreateOrUpdate(user))
                throw new AuthenticationException($"Cannot set logout status to '{user.Id}'");

            foreach (var chatId in user.ChatIds)
            {
                _messageSender.SendUserStatusMessage(chatId, user.Id, logoutTime, UserStatus.Offline);
            }
        }
    }
}
