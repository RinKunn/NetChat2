using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat2.Services
{
    public interface IAuthentication
    {
        void Login();
        void Logout();
    }
}
