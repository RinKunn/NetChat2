using System;
using System.Diagnostics;

namespace NetChat2.Models
{
    public class User
    {
        private readonly string _name;

        public string Name => _name;

        public User(string name)
        {
            _name = name;
        }
    }
}
