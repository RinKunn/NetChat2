using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetChat2.Models;

namespace NetChat2.Services
{
    public interface IChatService
    {
        int GetOnlineUsersCount(Chat chat);
    }
}
