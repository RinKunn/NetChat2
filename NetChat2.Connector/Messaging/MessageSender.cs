using System;
using System.IO;
using System.Text;

namespace NetChat2.Api
{
    public class MessageSender
    {
        private readonly string _filePath;
        private readonly Encoding _encoding;

        public MessageSender(string filePath, Encoding encoding)
        {
            _filePath = filePath;
            _encoding = encoding;
        }

        public void SendMessage(string userId, string textMessage, DateTime dateTime)
        {
            var message = new NetChatMessage(userId, textMessage, dateTime);
            File.AppendAllText(_filePath, message.ToString() + '\n', _encoding);
        }

        public void SendUserStatusMessage(string userId, bool isOnlineStatus, DateTime dateTime)
        {
            var message = new NetChatMessage(userId, isOnlineStatus ? "Logon" : "Logout", dateTime);
            File.AppendAllText(_filePath, message.ToString() + '\n', _encoding);
        }
    }
}
