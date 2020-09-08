using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;

namespace NetChat2.Api
{
    public static class FileHelper
    {
        /// <summary>
        /// Read last line of file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="encoding"></param>
        /// <param name="newline"></param>
        /// <returns></returns>
        public static string ReadLastLine(string path, Encoding encoding, string newline = "\n")
        {
            int charsize = encoding.GetByteCount("\n");
            byte[] buffer = encoding.GetBytes(newline);
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
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

        /// <summary>
        /// Read last <paramref name="lastLines"/> count of lines
        /// </summary>
        /// <param name="path"></param>
        /// <param name="encoding"></param>
        /// <param name="lastLines"></param>
        /// <param name="newline"></param>
        /// <returns></returns>
        public static string[] ReadAllLines(string path, Encoding encoding, int lastLines = 0, string newline = "\n")
        {
            int charsize = encoding.GetByteCount("\n");
            List<string> lines = new List<string>();
            byte[] buffer = encoding.GetBytes(newline);

            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                long endpos = stream.Length / charsize;
                long pos = charsize;
                int readedLinesCount = 0;
                while (pos <= endpos)
                {
                    stream.Seek(-pos, SeekOrigin.End);
                    stream.Read(buffer, 0, buffer.Length);
                    if (encoding.GetString(buffer) == newline && (stream.Length - stream.Position > charsize))
                        readedLinesCount++;
                    pos += charsize;
                    if (readedLinesCount >= lastLines && lastLines > 0)
                        break;
                }
                stream.Seek(-pos + 1, SeekOrigin.End);

                buffer = new byte[stream.Length - stream.Position];
                stream.Read(buffer, 0, buffer.Length);

                lines.AddRange(encoding.GetString(buffer).Split(new string[] { newline }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(line => Regex.Replace(line, @"[\u0000-\u001F]", string.Empty)));
            }

            return lines.ToArray();
        }


        /// <summary>
        /// Read async last <paramref name="lastLines"/> count of lines
        /// </summary>
        /// <param name="path"></param>
        /// <param name="encoding"></param>
        /// <param name="lastLines"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<string[]> ReadAllLinesAsync(string path, Encoding encoding, int lastLines = 0, CancellationToken token = default)
        {
            int DefaultBufferSize = 4096;
            FileOptions DefaultOptions = FileOptions.Asynchronous | FileOptions.SequentialScan;
            var lines = new List<string>();
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize, DefaultOptions))
            using (var reader = new StreamReader(stream, encoding))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    token.ThrowIfCancellationRequested();
                    lines.Add(line);
                }
            }
            return (lastLines == 0 ? lines.ToArray() : lines.Skip(Math.Max(0, lines.Count - lastLines)).ToArray());
        }
        
        

    }
}
