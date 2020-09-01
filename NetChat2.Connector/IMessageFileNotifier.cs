using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;

namespace NetChat2.Connector
{
    public delegate void OnMessageReceivedHandler(NetChatMessage message);

    public interface IMessageFileNotifier : IDisposable
    {
        event OnMessageReceivedHandler OnMessageReceived;
    }
}
