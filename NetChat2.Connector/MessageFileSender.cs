using System.IO;
using System;
using System.Text;

namespace NetChat2.Connector
{
    public class MessageFileSender
    {
        private readonly string _filePath;
        private readonly Encoding _encoding;

        public MessageFileSender(string filePath, Encoding encoding)
        {
            _filePath = filePath;
            _encoding = encoding;
        }

        public void SendMessage(NetChatMessage message)
        {
            File.AppendAllText(_filePath, message.ToString() + '\n', _encoding);
        }

        public void SendUserStatusMessage(string userId, DateTime dateTime, bool isOnlineStatus)
        {
            var message = new NetChatMessage(userId, isOnlineStatus ? "Logon" : "Logout", dateTime);
            File.AppendAllText(_filePath, message.ToString() + '\n', _encoding);
        }
    }
}
