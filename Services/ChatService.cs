using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using NetChat2.FileMessaging;
using NetChat2.Models;
using System.Threading;

namespace NetChat2.Services
{
    public delegate void NewMessageReceivedHandler(MessageViewModel message);

    public interface IMessengerService : IDisposable
    {
        event NewMessageReceivedHandler NewMessageReceived;

        Task SendMessageAsync(string message);
        Task ConnectAsync();
        
        Task<IEnumerable<MessageViewModel>> LoadAllMessages(int count = 0);
    }

    public class ChatService : IMessengerService
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public event NewMessageReceivedHandler NewMessageReceived;
        private CancellationTokenSource tokenSource;
        private readonly List<string> _activeUsers;
        private readonly INetchatHub _hub;
        private readonly User _user;

        public ChatService(INetchatHub hub, User user)
        {
            _user = user;
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
            logger.Debug("Recieved new message from '{0}': {1}", message.UserName, message.Text);
            NewMessageReceived?.Invoke(
                new MessageViewModel(
                    message.DateTime, 
                    new User(), 
                    message.Text, 
                    message.UserName == _user.Name,
                    message.UserName == _user.Name));
        }

        public async Task ConnectAsync()
        {
            await _hub.SendMessageAsync(new NetChatMessage(_user.Name, "Logon"));
        }

        public async Task SendMessageAsync(string message)
        {
            await _hub.SendMessageAsync(new NetChatMessage(_user.Name, message));
        }

        public async Task<IEnumerable<MessageViewModel>> LoadAllMessages(int count = 0)
        {
            var res = await _hub.LoadMessages(tokenSource.Token, 50);
            return res.Select(message => new MessageViewModel(message.DateTime, new User(), message.Text, message.UserName == _user.Name, true));
        }

        public void Dispose()
        {
            tokenSource.Cancel();
            logger.Debug("Disposing. Token cancelled...");
            _hub.SendMessage(new NetChatMessage(_user.Name, "Logout"));
            logger.Debug("Disposing. Message sended...");
            _hub.Dispose();
            logger.Debug("Disposing. Hub disposed...");
        }
    }
}
