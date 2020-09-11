using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetChat2.Models;
using NLog;

namespace NetChat2.Services
{
    public class InitingService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        IUserService userService;
        IChatService chatService;

        public InitingService(IUserService userService, IChatService chatService)
        {
            this.userService = userService;
            this.chatService = chatService;
        }

        public void InitIfNotInited(Chat chat)
        {
            if (userService.GetUsersCount() == 0)
            {
                logger.Info("Users store is empty. One time initing...");

                var users = chatService.LoadUsersFromChat(chat).ToList();
                string myId = userService.GetMyUserId();
                if (!users.Select(u => u.Id).Contains(myId))
                    users.Add(userService.GetMe());
                logger.Info("Loaded {0} users from chat '{1}'", users.Count, chat.ChatData.Title);

                userService.Create(users.ToArray());
                logger.Info("User store created!");
            }
        }
    }
}
