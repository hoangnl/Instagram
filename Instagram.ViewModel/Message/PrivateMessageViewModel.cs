using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Instagram.ViewModel.Feed
{
    public class PrivateMessageViewModel
    {
        public PrivateMessageViewModel(long messageId, string userName, string fromUserId, string toUserId, string fullName, string avatar, string body, DateTime createDate)
        {
            MessageId = messageId;
            FromUserId = fromUserId;
            ToUserId = toUserId;
            UserName = userName;
            FullName = fullName;
            Avatar = avatar;
            Body = body;
            CreateDate = createDate;
        }
        public long MessageId { get; set; }
        public string FromUserId { get; set; }
        public string ToUserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public string Body { get; set; }
        public System.DateTime CreateDate { get; set; }

    }
}
