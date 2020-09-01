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

    public class TextMessageViewModel : ObservableObject, IReadable
    {
        private DateTime _dateTime;
        private User _user;
        private string _text;
        private bool _isReaded;
        private bool _isOriginNative;

        public DateTime DateTime => _dateTime;
        public string Username => _user.Name;
        public string Text => _text;
        public bool IsReaded => _isReaded;
        public bool IsOriginNative => _isOriginNative;


        public TextMessageViewModel(DateTime dateTime, User user, string text, bool isOriginNative = false, bool isReaded = false)
        {
            _dateTime = dateTime;
            _user = user;
            _text = text;
            _isReaded = isReaded;
            _isOriginNative = isOriginNative;
        }
            
        public void Read()
        {
            if (IsReaded) return;
            Set(nameof(IsReaded), ref _isReaded, true);
        }

    }
}
