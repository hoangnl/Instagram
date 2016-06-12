using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Instagram.Model.EDM;
using Instagram.ViewModel.Feed;

namespace Instagram.Service.Message
{
    public interface IMessageService
    {
        MessageWrapperViewModel GetPagingMessage(string userId, int pageIndex, int pageSize);
        IEnumerable<MessageViewModel> GetMessageByUserId(string currentUserId, string userId);

        long SendMessage(string userId, string toUserId, string message);

        PrivateMessageViewModel GetMessageById(long messageId);
    }
}
