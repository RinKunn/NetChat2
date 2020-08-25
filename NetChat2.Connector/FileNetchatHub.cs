using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace NetChat2.Connector
{
    public class FileNetchatHub : INetchatHub
    {
        private readonly string _path;
        private readonly string _filename;
        private FileSystemWatcher _fileWatcher;

        public event OnMessageReceivedHandler OnMessageReceived;
        public string SourcePath => _path;

        public FileNetchatHub(string path)
        {
            _path = !string.IsNullOrWhiteSpace(path) ? path : throw new ArgumentNullException(nameof(path));
            _filename = Path.GetFileName(path);
            _fileWatcher = new FileSystemWatcher(Path.GetDirectoryName(path));
            _fileWatcher.Filter = _filename;
            _fileWatcher.NotifyFilter = NotifyFilters.LastWrite;
            _fileWatcher.Changed += OnFileChangedHandler;
            _fileWatcher.EnableRaisingEvents = true;
        }

        private void OnFileChangedHandler(object sender, FileSystemEventArgs e)
        {
            if (e.Name != _filename) return;
            string newLine = FileLastLineReader.ReadLastLine(_path);
            OnMessageReceived?.Invoke(new NetChatMessage(newLine));
        }


        public void SendMessage(NetChatMessage message)
        {
            File.AppendAllText(_path, message.ToString() + '\n');
        }

        public async Task SendMessageAsync(NetChatMessage message)
        {
            byte[] encodedText = Encoding.Unicode.GetBytes(message.ToString() + '\n');

            using (FileStream sourceStream = new FileStream(_path,
                FileMode.Append, FileAccess.Write, FileShare.None,
                bufferSize: 4096, useAsync: true))
            {
                await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
            };
        }

        public void Dispose()
        {
            _fileWatcher.EnableRaisingEvents = false;
            _fileWatcher.Changed -= OnFileChangedHandler;
            _fileWatcher.Dispose();
        }

        public async Task<IEnumerable<NetChatMessage>> LoadMessages()
        {
            var lines = File.ReadAllLines(_path);
            return lines.Select(l => new NetChatMessage(l));
        }
    }
}
