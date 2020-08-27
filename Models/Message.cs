using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace NetChat2.Models
{
    public interface IReadable
    {
        bool IsReaded { get; }
        void Read();
    }

    public class Message : ObservableObject, IReadable
    {
        private DateTime _dateTime;
        private User _user;
        private string _text;
        private bool _isReaded;

        public DateTime DateTime => _dateTime;
        public string Username => _user.Name;
        public string Text => _text;
        public bool IsReaded => _isReaded;
    
        public Message(DateTime dateTime, string username, string text)
        {
            _dateTime = dateTime;
            _user = new User(username);
            _text = text;
        }
            
        public void Read()
        {
            if (IsReaded) return;
            Set(ref _isReaded, true);
        }
    }
}
