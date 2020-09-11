using System;
using System.Collections.Generic;
using NetChat2.FileMessaging;
using NetChat2.Models;
using NetChat2.ViewModel;
using Locator = CommonServiceLocator.ServiceLocator;

namespace NetChat2.Services.Messaging
{
    public class NetChatMessageHub : IMessageHub
    {
        private readonly Dictionary<int, NotifierInfo> _notifiers = new Dictionary<int, NotifierInfo>();

        public void OnMessageReceived(Chat chat, Action<TextMessage> action)
        {
            var notifier = GetOrCreate(chat);
            notifier.AddHandler(action);
        }

        public void OnLoggedIn(Chat chat, Action<User> action)
        {
            var notifier = GetOrCreate(chat);
            notifier.AddLoginHandler(action);
        }

        public void OnLoggedOut(Chat chat, Action<User> action)
        {
            var notifier = GetOrCreate(chat);
            notifier.AddLogoutHandler(action);
        }


        private NotifierInfo GetOrCreate(Chat chat)
        {
            NotifierInfo notifier = null;
            if (!_notifiers.ContainsKey(chat.ChatData.Id))
            {
                notifier = new NotifierInfo(chat, Locator.Current.GetService<IUserService>());
                _notifiers.Add(chat.ChatData.Id, notifier);
            }
            else
                notifier = _notifiers[chat.ChatData.Id];
            return notifier;
        }

        public void Dispose()
        {
            foreach (var notifier in _notifiers.Values)
                notifier.Dispose();
        }



        private class NotifierInfo : IDisposable
        {
            private readonly IUserService userService;

            public Chat Chat { get; private set; }
            public MessageReceiver Receiver { get; private set; }

            private List<Action<TextMessage>> onMessageReceivedHandlers;
            private List<Action<User>> onUserLoggedInHandlers;
            private List<Action<User>> onUserLoggedOutHandlers;

            public NotifierInfo(Chat chat, IUserService userService)
            {
                Chat = chat;
                this.userService = userService;
                Receiver = new MessageReceiver(chat.ChatData.SourcePath, chat.ChatData.SourceEncoding);
                onMessageReceivedHandlers = new List<Action<TextMessage>>();
                onUserLoggedInHandlers = new List<Action<User>>();
                onUserLoggedOutHandlers = new List<Action<User>>();
                Receiver.OnMessageReceived += Receiver_OnMessageReceived;
                Receiver.OnUserLoggedIn += Receiver_OnUserLoggedIn;
                Receiver.OnUserLoggedOut += Receiver_OnUserLoggedOut;
            }

            private void Receiver_OnUserLoggedOut(OnUserStatusChangedArgs args)
            {
                var user = userService.GetUser(args.UserId);
                user.Status = UserStatus.Offline;
                user.StatusChangedDateTime = args.DateTime;
                foreach (var handler in onUserLoggedInHandlers)
                    handler?.Invoke(user);
            }

            private void Receiver_OnUserLoggedIn(OnUserStatusChangedArgs args)
            {
                var user = userService.GetUser(args.UserId);
                user.Status = UserStatus.Online;
                user.StatusChangedDateTime = args.DateTime;
                foreach (var handler in onUserLoggedInHandlers)
                    handler?.Invoke(user);
            }

            private void Receiver_OnMessageReceived(NetChatMessage message)
            {
                var textMessage = new TextMessage()
                    {
                        ChatId = Chat.ChatData.Id,
                        Text = message.Text,
                        Date = message.DateTime,
                        Sender = userService.GetUser(message.UserName)
                    };
                foreach (var hanlder in onMessageReceivedHandlers)
                    hanlder?.Invoke(textMessage);
            }

            public void AddHandler(Action<TextMessage> action)
            {
                onMessageReceivedHandlers.Add(action);
            }

            public void AddLoginHandler(Action<User> action)
            {
                onUserLoggedInHandlers.Add(action);
            }

            public void AddLogoutHandler(Action<User> action)
            {
                onUserLoggedOutHandlers.Add(action);
            }

            public void Dispose()
            {
                Receiver.OnMessageReceived -= Receiver_OnMessageReceived;
                Receiver.OnUserLoggedIn -= Receiver_OnUserLoggedIn;
                Receiver.OnUserLoggedOut -= Receiver_OnUserLoggedOut;
                onMessageReceivedHandlers.Clear();
                onUserLoggedInHandlers.Clear();
                onUserLoggedOutHandlers.Clear();
                Receiver.Dispose();
            }
        }
    }
}
