using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetChat2.Services
{
    public static class Helper
    {
        public static string[] ReadAllLines(string path, Encoding encoding, int lastLines = 0, CancellationToken token = default)
        {
            int DefaultBufferSize = 4096;
            FileOptions DefaultOptions = FileOptions.Asynchronous | FileOptions.SequentialScan;
            var lines = new List<string>();
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize, DefaultOptions))
            using (var reader = new StreamReader(stream, encoding))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    token.ThrowIfCancellationRequested();
                    lines.Add(line);
                }
            }
            return (lastLines == 0 ? lines.ToArray() : lines.Skip(Math.Max(0, lines.Count - lastLines)).ToArray());
        }
    }
}

