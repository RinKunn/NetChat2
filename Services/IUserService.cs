using System;
using NetChat2.Models;
using NetChat2.Services.Persistance;

namespace NetChat2.Services
{
    public interface IUserService
    {
        User GetMe();
        void Logon();
        void Logout();
        User GetUser(string envUserName);
    }
}
