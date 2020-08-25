using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat2.Models
{
    public class Message
    {
        private DateTime _dateTime;
        private User _user;
        private string _text;

        public DateTime DateTime => _dateTime;
        public string Username => _user.Name;
        public string Text => _text;

        public Message(DateTime dateTime, string username, string text)
        {
            _dateTime = dateTime;
            _user = new User(username);
            _text = text;
        }
    }
}
