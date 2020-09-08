using System;


namespace NetChat2.Api
{
    public class NetChatMessage
    {
        private const string DATE_FORMAT = "dd.MM HH:mm:ss";
        private const int USERNAME_AREA_SIZE = 10;

        private DateTime _datetime;
        private string _username;
        private string _message;

        public DateTime DateTime => _datetime;
        public string UserName => _username;
        public string Text => _message;

        public NetChatMessage(string username, string message)
        {
            _datetime = DateTime.Now;
            _username = username ?? throw new ArgumentNullException(nameof(username));
            _message = message.Replace("\n", " ").Replace("  ", " ");
        }

        public NetChatMessage(string username, string message, DateTime dateTime)
        {
            _datetime = dateTime;
            _username = username ?? throw new ArgumentNullException(nameof(username));
            _message = message.Replace("\n", " ").Replace("  ", " ");
        }

        public NetChatMessage(string line)
        {
            if (string.IsNullOrEmpty(line))
                throw new ArgumentNullException(line);
            _datetime = DateTime.ParseExact(line.Substring(0, DATE_FORMAT.Length), DATE_FORMAT, null);
            _username = line.Substring(DATE_FORMAT.Length + 1, line.IndexOf('>') - DATE_FORMAT.Length - 1).Trim();
            _message = line.Substring(line.IndexOf('>') + 2);
        }

        public override string ToString()
        {
            string res = string.Empty;
            if (_message.Contains("\n"))
            {
                var messages = _message.Split('\n');
                for (int i = 0; i < messages.Length; i++)
                    res += $"{_datetime.ToString(DATE_FORMAT)}|{_username.PadRight(USERNAME_AREA_SIZE)}> {_message}" + (i == messages.Length - 1 ? "" : "\n");
            }
            else
                res = $"{_datetime.ToString(DATE_FORMAT)}|{_username.PadRight(USERNAME_AREA_SIZE)}> {_message}";
            return res;
        }
    }
}
