using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace NetChat2.Connector
{
    public delegate void OnMessageReceivedHandler(NetChatMessage message);

    public interface INetchatHub : IDisposable
    {
        string SourcePath { get; }

        event OnMessageReceivedHandler OnMessageReceived;
        void SendMessage(NetChatMessage message);
        Task SendMessageAsync(NetChatMessage message);


        Task<IEnumerable<NetChatMessage>> LoadMessages();
    }
}
