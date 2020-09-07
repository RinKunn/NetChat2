using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat2.Persistance
{
    public class StoredChatData
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public string SourcePath { get; set; }
        public string SourceEncodingName { get; set; }

        public List<string> Members { get; set; }
    }

}
