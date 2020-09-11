using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetChat2.Models;

namespace NetChat2.Services
{
    public interface IUserService
    {
        string GetMyUserId();

        bool IsMe(string userId);

        User GetMe();

        User GetUser(string userId);

        int GetUsersCount();

        bool Create(params User[] users);
    }
}
