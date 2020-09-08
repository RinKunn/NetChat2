using System.Collections.Generic;
using System.Linq;
using NetChat2.Models;

namespace NetChat2.Services
{
    public class InitingService
    {
        IUserService userService;
        IChatService chatService;

        public InitingService(IChatService chatService, IUserService userService)
        {
            this.chatService = chatService;
            this.userService = userService;
        }

        public void InitIfNotInited()
        {
            var chat = chatService.GetChat(1);
            if (chat == null)
                chatService.CreateChat("NetChat2", "Общий чат ВБРР по финансам");

            if (userService.GetUsersCount() == 0)
            {
                string myId = userService.GetMyUserId();
                var users = chatService.LoadUsersFromChat(1).ToList();
                userService.CreateOrUpdate(users.ToArray());
            }
        }
    }
}
