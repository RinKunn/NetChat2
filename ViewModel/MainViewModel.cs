using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using NetChat2.Models;
using System.Collections.ObjectModel;
using NetChat2.Connector;
using NetChat2.Services;
using NetChat2.Commands;
using System.Threading.Tasks;
using System;
using System.Windows.Input;

namespace NetChat2.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IChatService _chatService;

        private ObservableCollection<Message> _messages = new ObservableCollection<Message>();


        public ObservableCollection<Message> Messages => _messages;

        private string _textMessage;
        public string TextMessage
        {
            get => _textMessage;
            set => Set(ref _textMessage, value);
        }

        private bool _isConnected;
        public bool IsConnected
        {
            get => _isConnected;
            private set => Set(ref _isConnected, value);
        }

        public MainViewModel(IChatService chatService)
        {
            _chatService = chatService;
            _chatService.NewMessageReceived += ChatService_NewMessageReceived;
        }

        private void ChatService_NewMessageReceived(Message message)
        {
            try
            {
                App.Current.Dispatcher.Invoke(() => _messages.Add(message));
            }
            catch { }
        }
        

        //Logon
        private IAsyncCommand _connectCommand;
        public IAsyncCommand ConnectCommand => _connectCommand ??
            (_connectCommand = new AsyncCommand(() => Connect(), (o) => CanLogon()));

        private async Task Connect()
        {
            await _chatService.ConnectAsync();
            IsConnected = true;
            var res = await _chatService.LoadAllMessages();
            _messages = new ObservableCollection<Message>(res);
            RaisePropertyChanged(nameof(Messages));
        }
        private bool CanLogon() => !IsConnected;


        // Logout
        private IAsyncCommand _logoutCommand;
        public IAsyncCommand LogoutCommand => _logoutCommand ?? 
            (_logoutCommand = new AsyncCommand(() => Logout(), (o) => CanLogout()));

        private async Task Logout()
        {
            await _chatService.LogoutAsync();
            IsConnected = false;
        }
        private bool CanLogout() => IsConnected;



        //Send message
        private IAsyncCommand _sendMessageCommand;
        public IAsyncCommand SendMessageCommand => _sendMessageCommand ??
            (_sendMessageCommand = new AsyncCommand(() => SendTextMessage(), (o) => CanSendTextMessage()));

        private async Task SendTextMessage()
        {
            await _chatService.SendMessageAsync(_textMessage);
        }
        private bool CanSendTextMessage()
        {
            return (!string.IsNullOrEmpty(TextMessage) && IsConnected);
        }

    }
}