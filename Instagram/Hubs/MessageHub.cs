using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Instagram.ViewModel.Feed;
using Instagram.Service.Message;
using System.Threading.Tasks;

namespace Instagram.Hubs
{
    public class MessageHub : Hub
    {
        static List<UserViewModel> ConnectedUsers = new List<UserViewModel>();
        private readonly IMessageService MessageService;

        public MessageHub()
        {
            MessageService = new MessageService();
        }

        public void ServerHello()
        {
            Clients.All.hello();
        }

        /// <summary>
        /// Hàm đăng kí kết nối đến Hub
        /// </summary>
        /// <param name="userId">Id của người đăng nhập</param>
        public void Connect(string userId)
        {
            var Id = Context.ConnectionId;

            UserViewModel connectedUser = new UserViewModel() { ConnectionId = Id, UserId = userId };
            if (!ConnectedUsers.Any(e => e.ConnectionId == Id))
            {
                ConnectedUsers.Add(connectedUser);
            }

            //Gọi đến hàm onConnected phía client gửi message
            Clients.Caller.onConnected(connectedUser.UserId, ConnectedUsers);
            //Gọi đến hàm onNewUserConnected của tất cả client trừ client gửi message
            Clients.AllExcept(connectedUser.ConnectionId).onNewUserConnected(connectedUser.UserId);
        }

        /// <summary>
        /// Gửi message đến người nhận
        /// </summary>
        /// <param name="toUserId">UserId của người nhận message</param>
        /// <param name="message">Nội dung message</param>
        public void SendPrivateMessage(string toUserId, string message, long messageId)
        {
            try
            {
                string fromConnectionId = Context.ConnectionId;
                string strfromUserId = (ConnectedUsers.Where(u => u.ConnectionId == fromConnectionId).Select(u => u.UserId).FirstOrDefault()).ToString();

                List<UserViewModel> FromUsers = ConnectedUsers.Where(u => u.UserId == strfromUserId).ToList();

                PrivateMessageViewModel privateMessage = MessageService.GetMessageById(messageId);

                //PrivateMessageViewModel privateMessage = new PrivateMessageViewModel(messageViewModel.MessageId, messageViewModel.User.UserName, messageViewModel.User.UserId, toUserId, messageViewModel.User.FullName, messageViewModel.User.Avatar.Replace("~", ""), messageViewModel.Body, messageViewModel.CreateDate);

                if (FromUsers.Count != 0)
                {
                    foreach (var FromUser in FromUsers)
                    {
                        Clients.Client(FromUser.ConnectionId).receivedPrivateMessage(privateMessage);
                    }
                }
                List<UserViewModel> ToUsers = ConnectedUsers.Where(x => x.UserId == toUserId).ToList();
                if (ToUsers.Count != 0)
                {
                    foreach (var ToUser in ToUsers)
                    {
                        Clients.Client(ToUser.ConnectionId).receivedPrivateMessage(privateMessage);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SendUserTypingRequest(string toUserId)
        {
            string strfromUserId = (ConnectedUsers.Where(u => u.ConnectionId == Context.ConnectionId).Select(u => u.UserId).FirstOrDefault());

            List<UserViewModel> ToUsers = ConnectedUsers.Where(x => x.UserId == toUserId).ToList();

            foreach (var ToUser in ToUsers)
            {
                Clients.Client(ToUser.ConnectionId).ReceiveTypingRequest(strfromUserId);
            }
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var item = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (item != null)
            {
                ConnectedUsers.Remove(item);
                if (!ConnectedUsers.Any(u => u.UserId == item.UserId))
                {
                    var id = item.UserId;
                    Clients.All.onUserDisconnected(id);
                }
            }
            return base.OnDisconnected(stopCalled);
        }
    }
}