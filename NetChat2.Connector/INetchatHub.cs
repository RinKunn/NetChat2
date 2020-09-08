﻿using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;

namespace NetChat2.Connector
{
    public delegate void OnMessageReceivedHandler(NetChatMessage message);

    public interface INetchatHub : IDisposable
    {
        string SourcePath { get; }

        event OnMessageReceivedHandler OnMessageReceived;
        void SendMessage(NetChatMessage message);
        Task SendMessageAsync(NetChatMessage message, CancellationToken token = default);
        Task<IEnumerable<NetChatMessage>> LoadMessages(CancellationToken token = default, int count = 0);
    }
}