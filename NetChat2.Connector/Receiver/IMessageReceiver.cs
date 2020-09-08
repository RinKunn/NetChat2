using System;

namespace NetChat2.Api
{
    public interface IMessageReceiver : IDisposable
    {
        event OnMessageReceivedHandler OnMessageReceived;
        event OnUserLoggedInHandler OnUserLoggedIn;
        event OnUserLoggedOutHandler OnUserLoggedOut;
    }
}
