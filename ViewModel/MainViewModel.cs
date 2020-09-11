using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using NetChat2.Commands;
using NetChat2.Models;
using NetChat2.Services;

namespace NetChat2.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly IMessengerService _chatService;

        private ObservableCollection<MessageViewModel> _messages;
        public ObservableCollection<MessageViewModel> Messages
        {
            get => _messages;
            private set => Set(ref _messages, value);
        }

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

        public MainViewModel(IMessengerService chatService)
        {
            _chatService = chatService;
            _chatService.NewMessageReceived += ChatService_NewMessageReceived;
        }

        private void ChatService_NewMessageReceived(MessageViewModel message)
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
            (_connectCommand = new RelayCommandAsync(Connect, (o) => CanLogon()));
        private async Task Connect()
        {
            await Task.Delay(1000);
            var res = await _chatService.LoadAllMessages(50);
            Messages = new ObservableCollection<MessageViewModel>(res.ToList());
            await _chatService.ConnectAsync();
            IsConnected = true;   
        }
        private bool CanLogon() => !IsConnected;


        // Logout
        private ICommand _logoutCommand;
        public ICommand LogoutCommand => _logoutCommand ?? 
            (_logoutCommand = new RelayCommand(Logout));
        private void Logout()
        {
            IsConnected = false;
            _chatService.Dispose();
            Console.WriteLine();
        }


        //Send message
        private IAsyncCommand _sendMessageCommand;
        public IAsyncCommand SendMessageCommand => _sendMessageCommand ??
            (_sendMessageCommand = new RelayCommandAsync(SendTextMessage, (o) => CanSendTextMessage()));
        private async Task SendTextMessage()
        {
            await _chatService.SendMessageAsync(_textMessage);
            TextMessage = string.Empty;
        }
        private bool CanSendTextMessage()
        {
            return (!string.IsNullOrEmpty(TextMessage) && IsConnected);
        }


        // Copy message
        private ICommand _copyMessageCommand;
        public ICommand CopyMessageCommand => _copyMessageCommand ??
            (_copyMessageCommand = new RelayCommand<MessageViewModel>(CopyMessage));
        private void CopyMessage(MessageViewModel message)
        {
            Clipboard.SetText(message.Text);
        }
    }
}