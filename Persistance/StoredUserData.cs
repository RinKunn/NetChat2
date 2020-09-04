using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NetChat2.Models;
using Newtonsoft.Json;

namespace NetChat2.Persistance
{
    public class StoredUserData
    {
        public string Id { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public int Status { get; set; }
        public DateTime StatusLastChanged { get; set; }

        public List<int> ChatsIds { get; set; }
    }
}
