using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetChat2.Persistance.UsersStore;
using NetChat2.Models;
using System.Diagnostics;

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

        private void InitUserId()
        {
            string userId = Environment.UserName.ToUpper();
            Properties.Settings.Default.UserId = userId;
            Properties.Settings.Default.Save();
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

        public bool IsMe(string userId)
        {
            return userId == GetMyUserId();
        }

        public User GetMe()
        {
            string userId = GetMyUserId();
            var userData = _userRepository.GetUserById(userId);
            if (userData == null)
                return CreateDefaultUser(userId);
            return UserDataToUser(userData);
        }

        public User GetUser(string userId)
        {
            var userData = _userRepository.GetUserById(userId);
            if (userData == null) return null;
            return UserDataToUser(userData);
        }

        public int GetUsersCount()
        {
            return _userRepository.Count();
        }

        public bool Create(params User[] users)
        {
            return _userRepository
                .CreateOrUpdate(users.Select(u => new StoredUserData()
                {
                    Id = u.Id,
                    Surname = u.Surname,
                    Name = u.Name,
                    Lastname = u.Lastname
                    //Status = (int)u.Status,
                    //StatusLastChanged = u.StatusChangedDateTime
                }).ToArray());
        }


        private User UserDataToUser(StoredUserData userData)
        {
            return new User()
            {
                Id = userData.Id,
                Surname = userData.Surname,
                Name = userData.Name,
                Lastname = userData.Lastname,
                Self = IsMe(userData.Id)
                //Status = (UserStatus)userData.Status,
                //StatusChangedDateTime = userData.StatusLastChanged,

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
    }
        
}
