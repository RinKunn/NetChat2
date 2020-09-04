using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NetChat2.Models;
using Newtonsoft.Json;

namespace NetChat2.Persistance
{
    public interface IUserRepository
    {
        StoredUserData[] GetAll();

        StoredUserData GetUserById(string envUserName); 
        
        void ChangeStatus(string envUserName, UserStatus userStatus);

        bool Add(string id, string surname, string name, string lastname, UserStatus userStatus, DateTime statusChangedDate);

        void IncludeToChat(string userId, int chatId);

        int OnlineUsersCount(IEnumerable<string> userIds = null);

        string GetMyUserId();
    }

}
