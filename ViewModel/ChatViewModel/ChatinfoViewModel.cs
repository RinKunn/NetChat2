using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using NetChat2.Models;
using NetChat2.Services;
using Locator = CommonServiceLocator.ServiceLocator;

namespace NetChat2.ViewModel
{
    public class ChatInfoViewModel : ViewModelBase
    {
        private bool _isVisible;

        public bool IsVisible
        {
            get => _isVisible;
            set => Set(ref _isVisible, value);
        }
        public string Title { get; }
        public string Description { get; }
        public ObservableCollection<User> Users { get; }

        public ChatInfoViewModel(Chat chat)
            : this(chat, Locator.Current.GetService<IChatService>()) { }

        private ChatInfoViewModel(Chat chat, IChatService chatService)
        {
            Title = chat.ChatData.Title;
            Description = chat.ChatData.Description;
            Users = new ObservableCollection<User>(chatService.LoadUsersFromChat(chat.ChatData.Id));
        }

        private ChatInfoViewModel() { }

        public static ChatInfoViewModel Hidden()
        {
            return new ChatInfoViewModel()
            {
                IsVisible = false
            };
        }


        private ICommand _closeCommand;
        public ICommand CloseCommand =>
            _closeCommand ??
            (_closeCommand = new RelayCommand(Close, IsVisible));

        private void Close()
        {
            this.IsVisible = false;
        }
    }
}
