﻿using System;
using System.Diagnostics;

namespace NetChat2.Models
{
    public class UserInfo
    {
        public string Id { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public UserStatus Status { get; set; }
        public DateTime StatusChangedDateTime { get; set; }
    }
}
