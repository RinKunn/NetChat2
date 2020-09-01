using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat2.Services.Persistance
{
    public class ChatData
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ChatPath { get; set; }
        public Encoding Encoding { get; set; }
        public List<string> Users { get; set; }
    }

}
