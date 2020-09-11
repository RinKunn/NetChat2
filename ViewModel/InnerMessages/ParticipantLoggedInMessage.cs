﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetChat2.Models;

namespace NetChat2.ViewModel.InnerMessages
{
    public class ParticipantLoggedInMessage : IInnerMessage
    {
        public User User { get; private set; }

        public ParticipantLoggedInMessage(User user)
        {
            User = user;
        }
    }
}
