using System;
using System.Diagnostics;

namespace NetChat2.Models
{
    public class User
    {
        public string EnvName { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }

        public bool Self { get; set; }

        public UserStatus Status { get; set; }
        public DateTime StatusChangedDateTime { get; set; }

        public User(string name)
        {
            EnvName = name;
        }
    }

    public enum UserStatus
    {
        Online, Offline
    }
}
