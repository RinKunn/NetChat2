using System;
using NetChat2.Models;
using NetChat2.Services.Persistance;

namespace NetChat2.Services
{
    public class DefaultUserService : IUserService
    {

        private readonly IUserRepository _userRepository;

        public DefaultUserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User GetMe()
        {
            string envUserName = Environment.UserName.ToUpper();
            var userData = _userRepository.GetUserByEnvName(envUserName);
            if (userData == null)
                return CreateDefaultUser(envUserName);

            return UserDataToUser(userData);
        }

        public void Logon()
        {
            _userRepository.Logon(Environment.UserName.ToUpper());
        }

        public void Logout()
        {
            _userRepository.Logout(Environment.UserName.ToUpper());
        }

        public User GetUser(string envUserName)
        {
            var userData = _userRepository.GetUserByEnvName(envUserName);
            if (userData == null)
                return CreateDefaultUser(envUserName);

            return UserDataToUser(userData);
        }




        private User UserDataToUser(UserData userData)
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
