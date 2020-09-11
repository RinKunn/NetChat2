using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetChat2.Models;

namespace NetChat2.Persistance.UsersStore
{
    public interface IUserRepository
    {
        StoredUserData GetUserById(string userId);

        bool CreateOrUpdate(params StoredUserData[] storedUsers);

        int Count();
    }
}
