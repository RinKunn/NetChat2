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
        private readonly IUserService _userService;

        public MainViewModel(IChatLoader chatLoader, IMessageHub messageHub, IUserService userService)
        {
            _userService = userService;
            var chat = chatLoader.LoadChat(1);
            if (chat == null) throw new ArgumentNullException(nameof(chat));
            MessengerViewModel = new MessengerViewModel(chat, messageHub, userService);
            
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
            _userService.Logon();
        }

        private void Logout()
        {
            _userService.Logout();
        }
    }
}