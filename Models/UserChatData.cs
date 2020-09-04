﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat2.Models
{
    public class UserChatData
    {
        public long Id { get; set; }
        public Chat Chat { get; set; }
        public User User { get; set; }
        public int UnreadCount { get; set; }
        public bool IsPinned { get; set; }
        public TextMessage LastMessage { get; set; }
    }
}
