﻿using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using NetChat2.Models;
using NetChat2.Services;
using NetChat2.ViewModel.Messages;
using Locator = CommonServiceLocator.ServiceLocator;

namespace NetChat2.ViewModel
{
    public class ChatHeaderViewModel : ViewModelBase
    {
        private int _participantCount;
        
        public string Title
        {
            get;
        }
        public int ParticipantCount
        {
            get => _participantCount;
            set => Set(ref _participantCount, value);
        }

        public ChatHeaderViewModel(Chat chat) : this(chat, Locator.Current.GetService<IChatLoader>()) { }

        private ChatHeaderViewModel(Chat chat, IChatLoader chatLoader)
        {
            this.Title = chat.Title;
            this.ParticipantCount = chatLoader.LoadChatUsers(chat).Length;
            MessengerInstance.Register<ParticipantLoggedInMessage>(this, (m) => ParticipantCount++);
            MessengerInstance.Register<ParticipantLoggedOutMessage>(this, (m) => ParticipantCount--);
        }

        private ICommand _getChatInfoCommand;
        public ICommand GetChatInfoCommand
            => _getChatInfoCommand ??
            (_getChatInfoCommand = new RelayCommand(OpenChatInfo));

        private void OpenChatInfo()
        {
            MessengerInstance.Send<ShowViewMessage>(new ShowViewMessage(), "info");
        }
    }
}
