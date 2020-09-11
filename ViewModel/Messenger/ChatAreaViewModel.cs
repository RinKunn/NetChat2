using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using NetChat2.Commands;
using NetChat2.Models;
using NetChat2.Services;
using NetChat2.ViewModel.InnerMessages;
using Locator = CommonServiceLocator.ServiceLocator;

namespace NetChat2.ViewModel
{
    public class ChatAreaViewModel : ViewModelBase
    {
        private readonly Chat _chat;
        private readonly IMessageLoader _messageLoader;
        private readonly IMessageSender _messageSender;

        private ObservableCollection<TextMessageViewModel> _messages;
        public ObservableCollection<TextMessageViewModel> Messages
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

        private bool _isLoaded;
        public bool IsLoaded
        {
            get => _isLoaded;
            private set => Set(ref _isLoaded, value);
        }


        internal ChatAreaViewModel()
        {

        }

        public ChatAreaViewModel(Chat chat) : this(
                chat,
                Locator.Current.GetService<IMessageSender>(),
                Locator.Current.GetService<IMessageLoader>())
        { }

        public ChatAreaViewModel(
            Chat chat,
            IMessageSender messageSender,
            IMessageLoader messageLoader)
        {
            _chat = chat;
            _messageLoader = messageLoader;
            _messageSender = messageSender;
            _isLoaded = false;
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
            _messageSender.SendMessage(
                new TextMessage()
                {
                    Date = DateTime.Now,
                    Sender = _chat.User,
                    Text = TextMessage,
                    ChatId = _chat.ChatData.Id
                });
            TextMessage = string.Empty;

        }
        private bool CanSendTextMessage()
        {
            return (!string.IsNullOrEmpty(TextMessage) && IsLoaded);
        }
        #endregion

        #region Load messages
        private IAsyncCommand _loadMessagesCommad;
        public IAsyncCommand LoadMessagesCommad =>
            _loadMessagesCommad ??
            (_loadMessagesCommad = new RelayCommandAsync(LoadMessagesAsync, (o) => Messages == null || Messages.Count == 0));

        private async Task LoadMessagesAsync()
        {
            try
            {
                var loadedMessages = await _messageLoader.LoadMessagesAsync(100);
                Messages = new ObservableCollection<TextMessageViewModel>(loadedMessages.Select(m => MessageToViewModel(m, true)));
                MessengerInstance.Register<MessageReceived>(this, (message) => HandleMessageReceived(message.Message));
                IsLoaded = true;
            }
            catch (Exception ex)
            {
                MessengerInstance.Unregister<MessageReceived>(this, _chat.ChatData.Title);
            }
        } 
        #endregion
    }
}
