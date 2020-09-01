using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NetChat2.Models;
using Newtonsoft.Json;

namespace NetChat2.Services.Persistance
{
    public interface IUserRepository
    {
        UserData[] GetAll();
        UserData GetUserByEnvName(string envUserName);
        void Logon(string envUserName);
        void Logout(string envUserName);
    }

}
