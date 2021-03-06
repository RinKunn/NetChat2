﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetChat2.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public UserStatus Status { get; set; }
        public DateTime StatusChangedDateTime { get; set; }

        public bool Self { get; set; }
        public List<int> ChatIds { get; set; }
    }

    public enum UserStatus
    {
        Offline = 0,
        Online = 1
    }
}
