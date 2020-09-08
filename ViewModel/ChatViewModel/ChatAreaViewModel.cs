using System;
using System.Collections.ObjectModel;
using System.Linq;
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
        private readonly IMessageLoader _messageLoader;
        private readonly IMessageSender _messageSender;
        
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
                Locator.Current.GetService<IMessageSender>(),
                Locator.Current.GetService<IMessageLoader>()) { }

        public ChatAreaViewModel(Chat chat,
            IMessageSender messageSender,
            IMessageLoader messageLoader)
        {
            _chat = chat;
            _messageLoader = messageLoader;
            _messageSender = messageSender;
            _isLoaded = false;

            _messages = LoadMessages();
        }

        private ObservableCollection<TextMessageViewModel> LoadMessages()
        {
            ObservableCollection<TextMessageViewModel> messages = null;
            try
            {
                messages = new ObservableCollection<TextMessageViewModel>(
                    _messageLoader.LoadMessages(1, 100)
                    .Select(m => MessageToViewModel(m, true)));
                MessengerInstance.Register<MessageReceived>(this, _chat.ChatData.Title, (message) => HandleMessageReceived(message.Message));
                IsLoaded = true;
            }
            catch (Exception ex)
            {
                MessengerInstance.Send<ThrowedExceptionMessage>(new ThrowedExceptionMessage(ex, $"Cannot load messages for chat '{_chat.ChatData.Title}'"));
                MessengerInstance.Unregister<MessageReceived>(this, _chat.ChatData.Title);
            }
            return messages ?? new ObservableCollection<TextMessageViewModel>();
        }

        private void HandleMessageReceived(TextMessage message)
        {
            if (string.IsNullOrEmpty(message.Text)) return;
            Messages.Add(MessageToViewModel(message, false));
        }

        private TextMessageViewModel MessageToViewModel(TextMessage message, bool isReaded = false)
        {
            return 
                new TextMessageViewModel(
                    message.Date,
                    message.Sender,
                    message.Text,
                    message.Sender.Id == _chat.User.Id,
                    isReaded);
        }


        #region Send message
        private ICommand _sendMessageCommand;
        public ICommand SendMessageCommand =>
            _sendMessageCommand ??
            (_sendMessageCommand = new RelayCommand(SendTextMessage, CanSendTextMessage));

        private void SendTextMessage()
        {
            //TODO handle send message exception
            _messageSender.SendMessage(_chat.ChatData.Id,
                new TextMessage()
                {
                    Date = DateTime.Now,
                    Sender = _chat.User,
                    Text = TextMessageViewModel,
                    ChatId = _chat.ChatData.Id
                });
            TextMessageViewModel = string.Empty;

        }
        private bool CanSendTextMessage()
        {
            return (!string.IsNullOrEmpty(TextMessageViewModel) && IsLoaded);
        } 
        #endregion
    }
}
