using System;

namespace NetChat2.Api
{
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
