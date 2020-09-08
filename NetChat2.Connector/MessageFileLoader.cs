using System;
using System.Linq;
using System.Text;

namespace NetChat2.Connector
{
    public class MessageFileLoader
    {
        private readonly string _filePath;
        private readonly Encoding _encoding;

        public MessageFileLoader(string filePath, Encoding encoding)
        {
            _filePath = filePath;
            _encoding = encoding;
        }

        public NetChatMessage[] LoadMessages(int limit = 0)
        {
            var lines = FileHelper.ReadAllLines(_filePath, _encoding, limit);
            return lines
                .Select(l => new NetChatMessage(l))
                .Where(m => m.Text != "Logon" && m.Text != "Logout")
                .ToArray();
        }

        public string[] GetUsersIds(int limit = 0)
        {
            var lines = FileHelper.ReadAllLines(_filePath, _encoding, limit);
            return lines
                .Select(l => new NetChatMessage(l))
                .Where(m => m.Text == "Logon" || m.Text == "Logout")
                .Select(m => m.UserName)
                .Distinct()
                .ToArray();
        }

        public UserStatusInfo[] GetUsersStatus(int limit = 0)
        {
            var lines = FileHelper.ReadAllLines(_filePath, _encoding, limit);
            var statusMessages = lines
                .Select(l => new NetChatMessage(l))
                .Where(m => m.Text == "Logon" || m.Text == "Logout")
                .OrderByDescending(m => m.DateTime)
                .ToArray();

            var users = statusMessages.Select(m => m.UserName).Distinct();
            return users
                .Select(u =>
                {
                    var message = statusMessages.First(m => m.UserName == u);
                    return new UserStatusInfo(message.UserName, message.Text == "Logon", message.DateTime);
                })
                .ToArray();
        }
    }

    public class UserStatusInfo
    {
        public UserStatusInfo(string userId, bool isOnline, DateTime statusChangedTime)
        {
            UserId = userId;
            IsOnline = isOnline;
            StatusChangedTime = statusChangedTime;
        }

        public string UserId { get; internal set; }
        public bool IsOnline { get; internal set; }
        public DateTime StatusChangedTime { get; internal set; }
    }
}
