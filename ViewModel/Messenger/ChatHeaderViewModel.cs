using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using NetChat2.Models;
using NetChat2.Services;
using NetChat2.ViewModel.InnerMessages;
using Locator = CommonServiceLocator.ServiceLocator;

namespace NetChat2.ViewModel
{
    public class ChatHeaderViewModel : ViewModelBase
    {
        private int _participantCount;

        public string Title { get; }

        public int ParticipantCount
        {
            get => _participantCount;
            set => Set(ref _participantCount, value);
        }

        internal ChatHeaderViewModel() { }

        public ChatHeaderViewModel(Chat chat)
            : this(chat, Locator.Current.GetService<IChatService>())
        { }

        private ChatHeaderViewModel(Chat chat, IChatService chatService)
        {
            this.Title = chat.ChatData.Title;
            this.ParticipantCount = chatService.GetOnlineUsersCount(chat);
            MessengerInstance.Register<ParticipantLoggedInMessage>(this, (m) => ParticipantCount++);
            MessengerInstance.Register<ParticipantLoggedOutMessage>(this, (m) => ParticipantCount--);
        }
    }
}
