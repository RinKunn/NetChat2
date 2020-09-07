using System;
using GalaSoft.MvvmLight;
using NetChat2.Services;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows.Input;

namespace NetChat2.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MessengerViewModel MessengerViewModel { get; private set; }
        private readonly IAuthentication _authentication;
        private readonly string _userId;

        public MainViewModel(IChatLoader chatLoader, IMessageHub messageHub, IAuthentication authentication, IUserService userService)
        {
            _authentication = authentication;
            var chat = chatLoader.LoadChat(1);
            if (chat == null) throw new ArgumentNullException(nameof(chat));
            MessengerViewModel = new MessengerViewModel(chat, messageHub, userService);
            _userId = userService.GetMyUserId();
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
            _authentication.Login(_userId);
        }

        private void Logout()
        {
            _authentication.Logout(_userId);
        }
    }
}