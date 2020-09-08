using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace NetChat2.Api
{

    [Obsolete("Use 'MessageReceiver' class", false)]
    public class FileNetchatHub : INetchatHub
    {
        private readonly Encoding encoding;
        private readonly string _path;
        private readonly string _filename;
        private FileSystemWatcher _fileWatcher;

        public event OnMessageReceivedHandler OnMessageReceived;
        public string SourcePath => _path;

        public FileNetchatHub(string path)
        {
            encoding = Encoding.GetEncoding(1251);
            _path = !string.IsNullOrWhiteSpace(path) ? Path.GetFullPath(path) : throw new ArgumentNullException(nameof(path));
            if (!File.Exists(_path)) File.Create(_path).Close();
            _filename = Path.GetFileName(_path);
            _fileWatcher = new FileSystemWatcher(Path.GetDirectoryName(_path));
            _fileWatcher.Filter = _filename;
            _fileWatcher.NotifyFilter = NotifyFilters.LastWrite;
            _fileWatcher.Changed += OnFileChangedHandler;
            _fileWatcher.EnableRaisingEvents = true;
        }

        private void OnFileChangedHandler(object sender, FileSystemEventArgs e)
        {
            if (e.Name != _filename) return;
            string newLine = FileHelper.ReadLastLine(_path, encoding);
            OnMessageReceived?.Invoke(new NetChatMessage(newLine));
        }

        public void SendMessage(NetChatMessage message)
        {
            File.AppendAllText(_path, message.ToString() + '\n', encoding);
        }

        public async Task SendMessageAsync(NetChatMessage message, CancellationToken token = default)
        {
            byte[] encodedText = encoding.GetBytes(message.ToString() + '\n');

            using (FileStream sourceStream = new FileStream(_path,
                FileMode.Append, FileAccess.Write, FileShare.None,
                bufferSize: 4096, useAsync: true))
            {
                await sourceStream.WriteAsync(encodedText, 0, encodedText.Length, token);
            };
        }

        public void Dispose()
        {
            Console.WriteLine($"Closing...");
            _fileWatcher.EnableRaisingEvents = false;
            _fileWatcher.Changed -= OnFileChangedHandler;
            //_fileWatcher.Dispose();
        }

        public async Task<IEnumerable<NetChatMessage>> LoadMessages(CancellationToken token = default, int count = 0)
        {
            var lines = await FileHelper.ReadAllLinesAsync(_path, encoding, count, token);
            return lines.Select(l => new NetChatMessage(l));
        }
    }
}
