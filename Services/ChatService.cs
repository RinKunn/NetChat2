using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using NetChat2.Connector;
using NetChat2.Models;
using System.Threading;

namespace NetChat2.Services
{
    public delegate void NewMessageReceivedHandler(Message message);

    public interface IChatService : IDisposable
    {
        event NewMessageReceivedHandler NewMessageReceived;

        Task SendMessageAsync(string message);
        Task ConnectAsync();
        
        Task<IEnumerable<Message>> LoadAllMessages(int count = 0);
    }

    public class ChatService : IChatService
    {
        public event NewMessageReceivedHandler NewMessageReceived;
        private CancellationTokenSource tokenSource;
        private readonly List<string> _activeUsers;
        private readonly INetchatHub _hub;
        private readonly User _user;

        public ChatService(INetchatHub hub)
        {
            _user = new User();
            _activeUsers = new List<string>();
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));
            _hub.OnMessageReceived += Hub_OnMessageReceived;
            tokenSource = new CancellationTokenSource();
        }

        private void Hub_OnMessageReceived(NetChatMessage message)
        {
            if (message.Text == "Logon" && !_activeUsers.Contains(message.UserName))
            {
                _activeUsers.Add(message.UserName);
            }
            else if (message.Text == "Logout" && _activeUsers.Contains(message.UserName))
            {
                _activeUsers.Remove(message.UserName);
            }

            NewMessageReceived?.Invoke(new Message(message.DateTime, message.UserName, message.Text));
        }

        public async Task ConnectAsync()
        {
            await _hub.SendMessageAsync(new NetChatMessage(_user.Name, "Logon"));
        }

        public async Task SendMessageAsync(string message)
        {
            await _hub.SendMessageAsync(new NetChatMessage(_user.Name, message));
        }

        public async Task<IEnumerable<Message>> LoadAllMessages(int count = 0)
        {
            var res = await _hub.LoadMessages(tokenSource.Token, 50);
            return res.Select(message => new Message(message.DateTime, message.UserName, message.Text, true));
        }

        public void Dispose()
        {
            tokenSource.Cancel();
            _hub.SendMessage(new NetChatMessage(_user.Name, "Logout"));
            _hub.Dispose();
        }
    }
}
