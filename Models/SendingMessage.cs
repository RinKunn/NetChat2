using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat2.Models
{
    public class SendingMessage
    {
        public DateTime CreatedDateTime { get; set; }
        public User Author { get; set; }
        public string MessageText { get; set; }
    }
}
