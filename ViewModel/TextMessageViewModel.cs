using System;
using GalaSoft.MvvmLight;
using NetChat2.Models;

namespace NetChat2.ViewModel
{

    public interface IReadable
    {
        bool IsReaded { get; }
        void Read();
    }

    public class TextMessageViewModel : ObservableObject, IReadable
    {
        private User _sender;
        private bool _isReaded;

        public DateTime DateTime { get; }
        public string SenderName => _sender.Name;
        public string Text { get; }
        public bool IsReaded => _isReaded;
        public bool IsOriginNative { get; }

        public TextMessageViewModel(DateTime dateTime, User sender, string text, bool isOriginNative = false, bool isReaded = false)
        {
            DateTime = dateTime;
            _sender = sender;
            Text = text;
            _isReaded = isReaded;
            IsOriginNative = isOriginNative;
        }

        public void Read()
        {
            if (IsReaded) return;
            Set(nameof(IsReaded), ref _isReaded, true);
        }
    }
}
