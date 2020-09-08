using System;
using System.Collections.Generic;
using NetChat2.Models;

namespace NetChat2.Persistance
{
    public interface IUserRepository
    {
        StoredUserData[] GetAll();

        StoredUserData GetUserById(string userId);

        bool Add(string id, string surname, string name, string lastname, UserStatus userStatus, DateTime statusChangedDate);

        bool Update(StoredUserData userData);

        void IncludeToChat(string userId, int chatId);

        int OnlineUsersCount(IEnumerable<string> userIds = null);

        int Count();

        bool CreateOrUpdate(params StoredUserData[] storedUsers);
    }

}
