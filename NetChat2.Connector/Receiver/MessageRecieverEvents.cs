using System;

namespace NetChat2.Api
{
    public delegate void OnMessageReceivedHandler(NetChatMessage message);
    public delegate void OnUserLoggedInHandler(OnUserStatusChangedArgs args);
    public delegate void OnUserLoggedOutHandler(OnUserStatusChangedArgs args);

    public class OnUserStatusChangedArgs
    {
        public string UserId { get; private set; }
        public DateTime DateTime { get; private set; }

        public OnUserStatusChangedArgs(string userId, DateTime datetime)
        {
            UserId = userId;
            DateTime = datetime;
        }
    }
}
