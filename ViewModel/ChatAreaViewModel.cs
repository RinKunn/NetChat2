using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using NetChat2.Models;
using NetChat2.Services;
using NetChat2.ViewModel.Messages;
using Locator = CommonServiceLocator.ServiceLocator;

namespace NetChat2.ViewModel
{
    public class ChatAreaViewModel : ViewModelBase
    {
        private readonly IMessageService _messageService;
        
        private readonly Chat _chat;

        private ObservableCollection<TextMessageViewModel> _messages;
        public ObservableCollection<TextMessageViewModel> Messages
        {
            get => _messages;
        }

        private string _textMessage;
        public string TextMessageViewModel
        {
            get => _textMessage;
            set => Set(ref _textMessage, value);
        }

        private bool _isLoaded;
        public bool IsLoaded
        {
            get => _isLoaded;
            private set => Set(ref _isLoaded, value);
        }

        public ChatAreaViewModel(Chat chat) : this(
                chat,
                Locator.Current.GetService<IMessageService>())
        { }

        public ChatAreaViewModel(
            Chat chat,
            IMessageService messageService)
        {
            _chat = chat;
            _messageService = messageService;
            _isLoaded = false;

            try
            {
                _messages = new ObservableCollection<TextMessageViewModel>(
                    _messageService.LoadMessages(1, 100)
                    .Select(m => new TextMessageViewModel(
                        m.Date,
                        m.Sender,
                        m.MessageText,
                        m.Sender.EnvName == _chat.User.EnvName,
                        true)));
                MessengerInstance.Register<MessageReceived>(this, _chat.Title, (message) => HandleMessageReceived(message.TextMessageViewModel));
                IsLoaded = true;
            }
            catch (Exception ex)
            {
                MessengerInstance.Send<ThrowedExceptionMessage>(new ThrowedExceptionMessage(ex, $"Cannot load messages for chat '{_chat.Title}'"));
                MessengerInstance.Unregister<MessageReceived>(this, _chat.Title);
            }
        }

        private void HandleMessageReceived(TextMessageViewModel message)
        {
            if (string.IsNullOrEmpty(message.Text)) return;
            Messages.Add(message);
        }

        private ICommand _sendMessageCommand;
        public ICommand SendMessageCommand =>
            _sendMessageCommand ??
            (_sendMessageCommand = new RelayCommand(SendTextMessage, CanSendTextMessage));

        private void SendTextMessage()
        {
            //TODO handle send message exception
            _messageService.SendMessage(_chat,
                new TextMessage()
                {
                    Date = DateTime.Now,
                    Sender = _chat.User,
                    MessageText = TextMessageViewModel
                });
            TextMessageViewModel = string.Empty;
            
        }
        private bool CanSendTextMessage()
        {
            return (!string.IsNullOrEmpty(TextMessageViewModel) && IsLoaded);
        }
    }
}
