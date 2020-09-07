using System;
using NetChat2.Models;
using NetChat2.Persistance;

namespace NetChat2.Services
{
    public interface IUserService
    {
        bool IsMe(string userId);

        User GetMe();

        User GetUser(string userId);

        string GetMyUserId();
    }
}
