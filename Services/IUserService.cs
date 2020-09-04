using System;
using NetChat2.Models;
using NetChat2.Persistance;

namespace NetChat2.Services
{
    public interface IUserService
    {
        bool IsMe(string envUserName);

        User GetMe();

        void Logon(string envUserName = null);

        void Logout(string envUserName = null);

        User GetUser(string envUserName);
    }
}
