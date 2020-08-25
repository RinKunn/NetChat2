using System;

namespace NetChat2.Models
{
    public class User
    {
        private readonly string _name;

        public string Name => _name;

        public User()
        {
            _name = Environment.UserName.ToUpper();
        }

        public User(string name)
        {
            _name = name;
        }
    }
}
