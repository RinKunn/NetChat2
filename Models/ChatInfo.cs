using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat2.Models
{
    public class ChatInfo
    {
        public int Id { get; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public IEnumerable<User> Participants { get; private set; }
    }
}
