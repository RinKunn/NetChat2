using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat2.Persistance.UsersStore
{
    public class UserNotExistsExecption : Exception
    {
        public string UserId { get; private set; }

        public UserNotExistsExecption(string userId)
            : base($"User id='{userId}' not exists")
        {
            UserId = userId;
        }
    }
}
