using System;

namespace NetChat2.FileMessaging
{
    public interface IMessageReceiver : IDisposable
    {
        event OnMessageReceivedHandler OnMessageReceived;
        event OnUserLoggedInHandler OnUserLoggedIn;
        event OnUserLoggedOutHandler OnUserLoggedOut;
    }
}
