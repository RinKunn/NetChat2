﻿using GalaSoft.MvvmLight;
using NetChat2.Models;
using NetChat2.Services;
using NetChat2.ViewModel.Messages;

namespace NetChat2.ViewModel
{
    public class MessengerViewModel : ViewModelBase
    {
        private ChatHeaderViewModel _header;
        private ChatAreaViewModel _area;
        private ChatInfoViewModel _info;


        public ChatHeaderViewModel Header
        {
            get => _header;
            private set => Set(ref _header, value);
        }

        public ChatAreaViewModel Area
        {
            get => _area;
            private set => Set(ref _area, value);
        }
        
        public ChatInfoViewModel Info
        {
            get => _info;
            private set => Set(ref _info, value);
        }


        public MessengerViewModel(Chat chat, IMessageHub hub, IUserService userService)
        {
            Header = new ChatHeaderViewModel(chat);
            Area = new ChatAreaViewModel(chat);
            Info = ChatInfoViewModel.Hidden();

            hub.Subscribe(chat.Id, (message) => MessengerInstance.Send<MessageReceived>(
                new MessageReceived(
                    message.DateTime,
                    userService.GetUser(message.UserName),
                    message.Text,
                    chat.User)
                ));

            MessengerInstance.Register<ShowViewMessage>(this, "info", (m) => Info = new ChatInfoViewModel(m.Chat));
        }
    }
}
