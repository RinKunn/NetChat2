using System;
using GalaSoft.MvvmLight;
using NetChat2.Services;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows.Input;
using NetChat2.ViewModel.InnerMessages;
using NetChat2.Models;

namespace NetChat2.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IAuthentication _authentication;
        private readonly IUserService _userService;
        private readonly Chat _chat;
        
        public MessengerViewModel MessengerViewModel { get; private set; }


        public MainWindowViewModel(
            Chat chat,
            IMessageHub messageHub,
            IAuthentication authentication,
            IUserService userService, 
            IChatService chatService)
        {
            (new InitingService(userService, chatService)).InitIfNotInited(chat);

            _authentication = authentication;
            _userService = userService;
            _chat = chat;
            MessengerViewModel = new MessengerViewModel(chat);
            SubcribeToEvents(messageHub);
        }

        private void SubcribeToEvents(IMessageHub messageHub)
        {
            messageHub.OnMessageReceived(_chat, (message) =>
                MessengerInstance.Send<MessageReceived>(new MessageReceived(message)));

            messageHub.OnLoggedIn(_chat, (user) =>
                {
                    MessengerInstance.Send<ParticipantLoggedInMessage>(new ParticipantLoggedInMessage(user));
                });

            messageHub.OnLoggedOut(_chat, (user) =>
                {
                    MessengerInstance.Send<ParticipantLoggedOutMessage>(new ParticipantLoggedOutMessage(user));
                });
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
    }
}
