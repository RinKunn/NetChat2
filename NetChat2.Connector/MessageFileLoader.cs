using System.Linq;
using System.Text;

namespace NetChat2.Connector
{
    public class MessageFileLoader
    {
        private readonly string _filePath;
        private readonly Encoding _encoding;

        public MessageFileLoader(string filePath, Encoding encoding)
        {
            _filePath = filePath;
            _encoding = encoding;
        }

        public NetChatMessage[] LoadMessages(int limit = 0)
        {
            var lines = FileHelper.ReadAllLines(_filePath, _encoding, limit);
            return lines.Select(l => new NetChatMessage(l)).ToArray();
        }
    }
}
