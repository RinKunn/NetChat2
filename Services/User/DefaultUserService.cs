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

        public DefaultUserService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            string userId = Properties.Settings.Default.UserId;

            if (string.IsNullOrEmpty(userId))
                InitUserId();
        }

        public User GetMe()
        {
            string userId = GetMyUserId();
            var userData = _userRepository.GetUserById(userId);
            if (userData == null)
                return CreateDefaultUser(userId);
            return UserDataToUser(userData);
        }

        public bool IsMe(string userId)
        {
            return userId == GetMyUserId();
        }

        public User GetUser(string userId)
        {
            var userData = _userRepository.GetUserById(userId);
            return UserDataToUser(userData);
        }

        public string GetMyUserId()
        {
            string userId = Properties.Settings.Default.UserId;
#if DEBUG
            var processesCount = Process.GetProcessesByName("NetChat2").Length;
            if (processesCount > 1) userId += processesCount;
#endif
            return userId;
        }



        private User UserDataToUser(StoredUserData userData)
        {
            if (userData == null) return null;
            return new User()
            {
                Id = userData.Id,
                Surname = userData.Surname,
                Name = userData.Name,
                Lastname = userData.Lastname,
                Status = (UserStatus)userData.Status,
                StatusChangedDateTime = userData.StatusLastChanged,
                Self = IsMe(userData.Id)
            };
        }

        private User CreateDefaultUser(string userId)
        {
            return new User()
            {
                Id = userId,
                Surname = userId,
                Name = userId,
                Lastname = userId,
                Status = UserStatus.Online,
                StatusChangedDateTime = DateTime.Now,
                Self = IsMe(userId)
            };
        }

        private void InitUserId()
        {
            string userId = Environment.UserName.ToUpper();
            Properties.Settings.Default.UserId = userId;
            Properties.Settings.Default.Save();
        }
    }
}
