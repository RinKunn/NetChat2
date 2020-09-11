using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat2.Persistance.UsersStore
{
    public class StoredUserData
    {
        public string Id { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        //public int Status { get; set; }
        //public DateTime StatusLastChanged { get; set; }
    }
}
