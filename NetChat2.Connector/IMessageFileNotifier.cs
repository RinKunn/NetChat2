using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;

namespace NetChat2.Connector
{
    public delegate void OnMessageReceivedHandler(NetChatMessage message);
    public delegate void OnUserLoggedInHandler(OnUserStatusChangedArgs args);
    public delegate void OnUserLoggedOutHandler(OnUserStatusChangedArgs args);


    public interface IMessageFileNotifier : IDisposable
    {
        event OnMessageReceivedHandler OnMessageReceived;
        event OnUserLoggedInHandler OnUserLoggedIn;
        event OnUserLoggedOutHandler OnUserLoggedOut;
    }

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
