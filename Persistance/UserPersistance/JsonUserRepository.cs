using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NetChat2.Models;
using Newtonsoft.Json;

namespace NetChat2.Persistance
{
    public class JsonUserRepository : IUserRepository
    {
        private readonly string _path;

        public JsonUserRepository(string path)
        {
            _path = string.IsNullOrWhiteSpace(path) ? throw new ArgumentNullException(path) : path;
        }

        public StoredUserData[] GetAll()
        {
            if (!File.Exists(_path)) return null;
            return LoadData().Values.ToArray();
        }

        public StoredUserData GetUserById(string userId)
        {
            if (!File.Exists(_path)) return null;
            if (string.IsNullOrEmpty(userId)) throw new ArgumentNullException(nameof(userId));

            var users = LoadData();
            if (!users.ContainsKey(userId)) return null;

            return users[userId];
        }

        public bool Add(string userId, string surname, string name, string lastname,
            UserStatus userStatus, DateTime statusChangedDate)
        {
            if (string.IsNullOrEmpty(userId)) throw new ArgumentNullException(nameof(userId));
            if (string.IsNullOrEmpty(surname)) throw new ArgumentNullException(nameof(surname));

            var users = LoadData();

            if (users.ContainsKey(userId)) return false;
            users.Add(userId, new StoredUserData()
            {
                Id = userId,
                Surname = surname,
                Name = name,
                Lastname = lastname,
                Status = (int)userStatus,
                StatusLastChanged = statusChangedDate
            });
            SaveData(users);
            return true;
        }

        public bool Update(StoredUserData userData)
        {
            if (userData == null || string.IsNullOrEmpty(userData.Id)) throw new ArgumentNullException(nameof(userData));

            var users = LoadData();

            if (!users.ContainsKey(userData.Id)) return false;
            users[userData.Id] = userData;
            SaveData(users);
            return true;
        }


        //public void ChangeStatus(string userId, UserStatus status)
        //{
        //    if (string.IsNullOrEmpty(userId)) throw new ArgumentNullException(nameof(userId));

        //    Dictionary<string, StoredUserData> users = LoadData();

        //    if (!users.ContainsKey(userId)) throw new UserNotExistsExecption(userId);

        //    users[userId].Status = (int)status;
        //    users[userId].StatusLastChanged = DateTime.Now;

        //    SaveData(users);
        //}

        public int OnlineUsersCount(IEnumerable<string> userIds = null)
        {
            var users = LoadData();
            if (users.Count == 0) return 0;
            return users
                .Where(pair => userIds.Contains(pair.Key))
                .Sum(s => (int)s.Value.Status);
        }

        public void IncludeToChat(string userId, int chatId)
        {
            if (chatId < 0) throw new ArgumentNullException(nameof(chatId));
            var users = LoadData();

            if (!users.ContainsKey(userId)) 
                throw new UserNotExistsExecption(userId);

            if(!users[userId].ChatsIds.Contains(chatId))
            {
                users[userId].ChatsIds.Add(chatId);
                SaveData(users);
            }
        }

        private Dictionary<string, StoredUserData> LoadData()
        {
            return
                File.Exists(_path)
                ? JsonConvert.DeserializeObject<Dictionary<string, StoredUserData>>(File.ReadAllText(_path))
                : new Dictionary<string, StoredUserData>();
        }

        private void SaveData(Dictionary<string, StoredUserData> users)
        {
            FileAttempts.TryWriteAllText(_path, JsonConvert.SerializeObject(users), 1);
        }   
    }
}
