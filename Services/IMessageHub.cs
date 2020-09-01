using System;
using System.Text;
using NetChat2.Connector;

namespace NetChat2.Services
{
    public interface IMessageHub : IDisposable
    {
        void OnMessageReceived(Action<NetChatMessage> action);
    }

    public class DefaultMessageHub : IMessageHub
    {
        private Action<NetChatMessage> _action;
        private MessageFileNotifier _notifier;

        public DefaultMessageHub(string path, Encoding encoding)
        {
            _notifier = new MessageFileNotifier(path, encoding);
            _notifier.OnMessageReceived += OnMessageReceived;
        }

        private void OnMessageReceived(NetChatMessage message)
        {
            _action?.Invoke(message);
        }

        public void OnMessageReceived(Action<NetChatMessage> action)
        {
            _action = action;
        }

        public void Dispose()
        {
            _notifier.Dispose();
        }
    }
}
