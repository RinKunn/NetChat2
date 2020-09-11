using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using NetChat2.Models;

namespace NetChat2.ViewModel
{
    public class MessengerViewModel : ViewModelBase
    {
        private ChatHeaderViewModel _header;
        private ChatAreaViewModel _area;

        public ChatHeaderViewModel Header
        {
            get => _header;
            private set => Set(ref _header, value);
        }

        public ChatAreaViewModel Area
        {
            get => _area;
            private set => Set(ref _area, value);
        }

        public MessengerViewModel(Chat chat)
        {
            Header = new ChatHeaderViewModel(chat);
            Area = new ChatAreaViewModel(chat);
        }
    }
}
