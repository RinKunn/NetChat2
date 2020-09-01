using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;

namespace NetChat2.ViewModel.Messages
{
    public class ParticipantLoggedInMessage : IMessage
    {
        public int UserId;
    }
}
