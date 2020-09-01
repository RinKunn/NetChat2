using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using NetChat2.Commands;
using NetChat2.Models;
using NetChat2.Services;

namespace NetChat2.ViewModel
{
    public class ChatViewModel : ViewModelBase
    {
        private readonly IMessageSender _messageSender;
        private readonly IMessageLoader _messageLoader;
        private readonly User _currentUser;

        private ObservableCollection<Message> _messages = new ObservableCollection<Message>();
        public ObservableCollection<Message> Messages
        {
            get => _messages;
            set => Set(ref _messages, value);
        }

        private string _textMessage;
        public string TextMessage
        {
            get => _textMessage;
            set => Set(ref _textMessage, value);
        }

        private bool _isLoaded;
        public bool IsLoaded
        {
            get => _isLoaded;
            set => Set(ref _isLoaded, value);
        }

        public ChatViewModel(
            IMessageLoader messageLoader,
            IMessageSender messageSender, 
            IUserService userService
            )
        {
            _messageSender = messageSender;
            _messageLoader = messageLoader;
            _isLoaded = false;
            _currentUser = userService.GetCurrentUser();
            MessengerInstance.Register<Message>(null, (message) => Messages.Add(message));
        }

        private async Task Init()
        {
            await Task.Delay(1000);
            Messages = new ObservableCollection<Message>(_messageLoader.LoadMessages(0, 100));
            IsLoaded = true;
        }

        private ICommand _sendMessageCommand;
        public ICommand SendMessageCommand => 
            _sendMessageCommand ??
            (_sendMessageCommand = new RelayCommand(SendTextMessage, CanSendTextMessage));

        private void SendTextMessage()
        {
            //TODO handle send message exception
            _messageSender.SendMessage(
                new SendingMessage()
                {
                    CreatedDateTime = DateTime.Now,
                    Author = _currentUser,
                    MessageText = TextMessage
                });
            TextMessage = string.Empty;
        }
        private bool CanSendTextMessage()
        {
            return (!string.IsNullOrEmpty(TextMessage) && IsLoaded);
        }
    }
}
