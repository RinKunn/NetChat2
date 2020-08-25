using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace NetChat2.Connector
{
    public static class FileLastLineReader
    {
        public static string ReadLastLine(string path)
        {
            return ReadLastLine(path, Encoding.ASCII, "\n");
        }

        public static string ReadLastLine(string path, Encoding encoding, string newline)
        {
            int charsize = encoding.GetByteCount("\n");
            byte[] buffer = encoding.GetBytes(newline);
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                long endpos = stream.Length / charsize;

                for (long pos = charsize; pos <= endpos; pos += charsize)
                {
                    stream.Seek(-pos, SeekOrigin.End);
                    stream.Read(buffer, 0, buffer.Length);

                    if (encoding.GetString(buffer) == newline && stream.Length - stream.Position > charsize)
                    {
                        buffer = new byte[stream.Length - stream.Position];
                        stream.Read(buffer, 0, buffer.Length);
                        return Regex.Replace(encoding.GetString(buffer), @"[\u0000-\u001F]", string.Empty);
                    }
                    if (pos == endpos)
                    {
                        stream.Seek(-pos, SeekOrigin.End);
                        buffer = new byte[stream.Length];
                        stream.Read(buffer, 0, buffer.Length);
                        return Regex.Replace(encoding.GetString(buffer), @"[\u0000-\u001F]", string.Empty);
                    }
                }
            }
            return null;
        }
    }
}
