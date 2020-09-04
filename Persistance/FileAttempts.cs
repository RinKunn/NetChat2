using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat2.Persistance
{
    public static class FileAttempts
    {
        //TODO add write attempts

        public static void TryWriteAllText(string path, string text, int attempts = 1)
        {
            File.WriteAllText(path, text);
        }
    }
}
