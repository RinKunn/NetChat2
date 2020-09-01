using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using NetChat2.Models;

namespace NetChat2.ViewModel
{
    public class ChatinfoViewModel : ViewModelBase
    {
        private bool _isVisible;
        private string _title;
        private string _description;
        private readonly ObservableCollection<User> _users = new ObservableCollection<User>();

        public bool IsVisible
        {
            get => _isVisible;
            set => Set(ref _isVisible, value);
        }
        public string Title => _title;
        public string Description => _description;
        public ObservableCollection<User> Users => _users;

        public ChatinfoViewModel()
        {

        }
    }
}
