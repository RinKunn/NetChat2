using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetChat2.Connector;
using NetChat2.Models;

namespace NetChat2.Services
{
    public interface IUserService
    {
        User GetCurrentUser();
    }

    public class UserService
    {
        private readonly INetchatHub _hub;

        public UserService(INetchatHub hub)
        {
            _hub = hub;
        }

        public void Logon()
        {

        }

        public void Logout()
        {

        }
    }
}
