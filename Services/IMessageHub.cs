using System;
using System.Collections.Generic;
using System.Text;
using NetChat2.Connector;
using NetChat2.Persistance;


namespace NetChat2.Services
{
    public interface IMessageHub : IDisposable
    {
        void Subscribe(int chatId, Action<NetChatMessage> action);
        void Unsubscribe(int chatId);
    }

    public class DefaultMessageHub : IMessageHub
    {
        private readonly Dictionary<int, NotifierInfo> _notifiers;
        private readonly IChatRepository _chatRepository;

        public DefaultMessageHub(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
            _notifiers = new Dictionary<int, NotifierInfo>();
        }


        public void Subscribe(int chatId, Action<NetChatMessage> action)
        {
            OnMessageReceivedHandler handler = (netMessage) => action?.Invoke(netMessage);

            if (_notifiers.ContainsKey(chatId))
            {
                var subscriptionInfo = _notifiers[chatId];
                subscriptionInfo.Notifier.OnMessageReceived -= subscriptionInfo.Handler;

                subscriptionInfo.Notifier.OnMessageReceived += handler;
                subscriptionInfo.Handler = handler;
                return;
            }

            var chatData = _chatRepository.GetChatById(chatId);
            var notifier = new MessageFileNotifier(chatData.ChatPath, Encoding.GetEncoding(chatData.EncodingName));
            notifier.OnMessageReceived += handler;
            _notifiers.Add(chatId, new NotifierInfo() { Notifier = notifier, Handler = handler});
        }

        public void Unsubscribe(int chatId)
        {
            if (!_notifiers.ContainsKey(chatId)) return;
            var subscriptionInfo = _notifiers[chatId];
            subscriptionInfo.Notifier.OnMessageReceived -= subscriptionInfo.Handler;
            _notifiers.Remove(chatId);
        }

        public void Dispose()
        {
            foreach(var subscriptionInfo in _notifiers.Values)
            {
                subscriptionInfo.Notifier.OnMessageReceived -= subscriptionInfo.Handler;
                subscriptionInfo.Notifier.StopWatching();
            }
            _notifiers.Clear();
        }

        private struct NotifierInfo
        {
            public MessageFileNotifier Notifier { get; set; }
            public OnMessageReceivedHandler Handler { get; set; }
        }
    }
    

}
