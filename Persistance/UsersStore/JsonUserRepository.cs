using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetChat2.Models;
using Newtonsoft.Json;

namespace NetChat2.Persistance.UsersStore
{
    public class JsonUserRepository : IUserRepository
    {
        private readonly string _path;

        public JsonUserRepository(string path)
        {
            _path = string.IsNullOrWhiteSpace(path) ? throw new ArgumentNullException(path) : path;
        }


        public StoredUserData GetUserById(string userId)
        {
            if (!File.Exists(_path) || (new FileInfo(_path)).Length == 0) return null;
            if (string.IsNullOrEmpty(userId)) throw new ArgumentNullException(nameof(userId));

            var users = LoadData();
            if (!users.ContainsKey(userId)) return null;

            return users[userId];
        }

        public int Count()
        {
            return LoadData().Count;
        }

        public bool CreateOrUpdate(params StoredUserData[] storedUsers)
        {
            var users = LoadData();
            foreach(var updateUser in storedUsers)
            {
                if (users.ContainsKey(updateUser.Id))
                    users[updateUser.Id] = updateUser;
                else
                    users.Add(updateUser.Id, updateUser);
            }
            return SaveData(users);
        }


        private Dictionary<string, StoredUserData> LoadData()
        {
            return
                File.Exists(_path)
                ? JsonConvert.DeserializeObject<Dictionary<string, StoredUserData>>(File.ReadAllText(_path))
                : new Dictionary<string, StoredUserData>();
        }

        private bool SaveData(Dictionary<string, StoredUserData> users)
        {
            FileAttempts.TryWriteAllText(_path, JsonConvert.SerializeObject(users), 1);
            return true;
        }
    }
}
