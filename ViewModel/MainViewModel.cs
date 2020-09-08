using System;
using GalaSoft.MvvmLight;
using NetChat2.Services;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows.Input;
using NetChat2.ViewModel.Messages;
using NetChat2.Models;

namespace NetChat2.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IAuthentication _authentication;
        private readonly IMessageHub messageHub;
        private readonly IUserService userService;
        private readonly Chat _chat;

        public MessengerViewModel MessengerViewModel { get; private set; }


        public MainViewModel(
            IChatService chatService, 
            IMessageHub messageHub, 
            IAuthentication authentication, 
            IUserService userService)
        {
            var init = new InitingService(chatService, userService);
            init.InitIfNotInited();

            _authentication = authentication;
            var chat = chatService.GetChat(1);
            if (chat == null) throw new ArgumentNullException(nameof(chat));

            MessengerViewModel = new MessengerViewModel(chat);
            Subcribe(chat);
        }

        private ICommand _logonCommand;
        private ICommand LogonCommand =>
            _logonCommand ??
            (_logonCommand = new RelayCommand(Logon));


        private ICommand _logoutCommand;
        private ICommand LogoutCommand =>
            _logoutCommand ??
            (_logoutCommand = new RelayCommand(Logout));


        private void Logon()
        {
            _authentication.Login();
        }

        private void Logout()
        {
            _authentication.Logout();
        }


        private void Subcribe(Chat chat)
        {//TODO переделать
            messageHub.Subscribe(chat.ChatData.Id, (message) =>
            {
                MessengerInstance.Send<MessageReceived>(
                    new MessageReceived(new TextMessage()
                    {
                        ChatId = chat.ChatData.Id,
                        Date = message.DateTime,
                        Sender = userService.GetUser(message.UserName),
                        Text = message.Text
                    }));
            });
        }
    }
}