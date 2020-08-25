using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetChat2.Connector;
using NetChat2.Models;

namespace NetChat2.Services
{
    public delegate void NewMessageReceivedHandler(Message message);

    public interface IChatService
    {
        event NewMessageReceivedHandler NewMessageReceived;

        Task SendMessageAsync(string message);
        
        Task ConnectAsync();
        Task LogoutAsync();

        Task<IEnumerable<Message>> LoadAllMessages();
    }

    public class ChatService : IChatService
    {
        public event NewMessageReceivedHandler NewMessageReceived;
        private readonly List<string> _activeUsers;
        private readonly INetchatHub _hub;
        private readonly User _user;

        public ChatService(INetchatHub hub)
        {
            _user = new User();
            _activeUsers = new List<string>();
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));
            _hub.OnMessageReceived += Hub_OnMessageReceived;
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

        public async Task LogoutAsync()
        {
            await _hub.SendMessageAsync(new NetChatMessage(_user.Name, "Logout"));
        }

        public async Task SendMessageAsync(string message)
        {
            await _hub.SendMessageAsync(new NetChatMessage(_user.Name, message));
        }

        public async Task<IEnumerable<Message>> LoadAllMessages()
        {
            var res = await _hub.LoadMessages();
            return res.Select(message => new Message(message.DateTime, message.UserName, message.Text));
        }
    }
}
