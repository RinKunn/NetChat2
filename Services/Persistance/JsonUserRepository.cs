using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NetChat2.Models;
using Newtonsoft.Json;

namespace NetChat2.Services.Persistance
{

    public class JsonUserRepository : IUserRepository
    {
        private readonly string _path;

        public JsonUserRepository(string path)
        {
            _path = string.IsNullOrWhiteSpace(path) ? throw new ArgumentNullException(path) : path;
        }


        public UserData[] GetAll()
        {
            if (!File.Exists(_path)) return null;
            return JsonConvert.DeserializeObject<Dictionary<string, UserData>>(File.ReadAllText(_path)).Values.ToArray();
        }

        public UserData GetUserByEnvName(string envUserName)
        {
            if (!File.Exists(_path)) return null;
            var users = JsonConvert.DeserializeObject<Dictionary<string, UserData>>(File.ReadAllText(_path));
            return users[envUserName];
        }

        public void Logon(string envUserName)
        {
            ChangeStatus(envUserName, UserStatus.Online);
        }

        public void Logout(string envUserName)
        {
            ChangeStatus(envUserName, UserStatus.Offline);
        }


        private void ChangeStatus(string envUserName, UserStatus status)
        {
            Dictionary<string, UserData> users = null;
            if (File.Exists(_path))
                users = JsonConvert.DeserializeObject<Dictionary<string, UserData>>(File.ReadAllText(_path));
            else
                users = new Dictionary<string, UserData>();
            if (!users.ContainsKey(envUserName))
            {
                users.Add(envUserName, new UserData()
                {
                    EnvName = envUserName,
                    Surname = envUserName,
                    Name = string.Empty,
                    Lastname = string.Empty,
                    Status = (int)status,
                    StatusLastChanged = DateTime.Now
                });
            }
            else
            {
                users[envUserName].Status = (int)status;
                users[envUserName].StatusLastChanged = DateTime.Now;
            }
            var text = JsonConvert.SerializeObject(users);
            File.WriteAllText(_path, text);
        }
    }
}
