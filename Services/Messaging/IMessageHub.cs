using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetChat2.Models;

namespace NetChat2.Services
{
    public interface IMessageHub : IDisposable
    {
        void OnMessageReceived(Chat chat, Action<TextMessage> action);
        void OnLoggedIn(Chat chat, Action<User> action);
        void OnLoggedOut(Chat chat, Action<User> action);
    }
}
