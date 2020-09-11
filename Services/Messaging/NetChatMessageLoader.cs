using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetChat2.Models;

namespace NetChat2.Services.Messaging
{
    public class NetChatMessageLoader : IMessageLoader
    {
        public readonly IUserService _userService;

        public NetChatMessageLoader(IUserService userService)
        {
            _userService = userService;
        }

        public List<TextMessage> LoadMessages(Chat chat, int limit = 0)
        {
            var loader = new Api.MessageLoader(chat.ChatData.SourcePath, chat.ChatData.SourceEncoding);

            return loader.LoadMessages(limit)?
                .Select(netMessage =>
                    new TextMessage()
                    {
                        Date = netMessage.DateTime,
                        Text = netMessage.Text,
                        ChatId = chat.ChatData.Id,
                        Sender = _userService.GetUser(netMessage.UserName)
                    })
                .ToList();
        }

        public async Task<List<TextMessage>> LoadMessagesAsync(Chat chat, int limit = 0)
        {
            var loader = new Api.MessageLoader(chat.ChatData.SourcePath, chat.ChatData.SourceEncoding);

            var mess = await loader.LoadMessagesAsync(limit);
            return mess?.Select(netMessage =>
                    new TextMessage()
                    {
                        Date = netMessage.DateTime,
                        Text = netMessage.Text,
                        ChatId = chat.ChatData.Id,
                        Sender = _userService.GetUser(netMessage.UserName)
                    })
                .ToList();
        }
    }
}
