using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat2.Models
{
    public class Message
    {
        public DateTime Date { get; set; }
        public User Sender { get; set; }
        public int ChatId { get; set; }
    }
}
