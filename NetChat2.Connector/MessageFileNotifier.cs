using System;
using System.IO;
using System.Text;

namespace NetChat2.Connector
{
    public class MessageFileNotifier : IMessageFileNotifier
    {
        private readonly Encoding _encoding;
        private readonly string _path;
        private readonly string _filename;
        private FileSystemWatcher _fileWatcher;

        public event OnMessageReceivedHandler OnMessageReceived;
        
        public MessageFileNotifier(string path, Encoding encoding)
        {
            _encoding = encoding;
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
            string newLine = FileHelper.ReadLastLine(_path, _encoding);
            OnMessageReceived?.Invoke(new NetChatMessage(newLine));
        }

        public void StopWatching()
        {
            _fileWatcher.EnableRaisingEvents = false;
            _fileWatcher.Changed -= OnFileChangedHandler;
        }

        public void Dispose()
        {
            StopWatching();
            _fileWatcher.Dispose();
        }
    }
}
