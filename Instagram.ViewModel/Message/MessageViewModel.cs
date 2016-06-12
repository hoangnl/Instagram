using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.ViewModel.Feed
{
    public class MessageViewModel
    {
        public long MessageId { get; set; }
        public string FromUserId { get; set; }
        public string Body { get; set; }
        public System.DateTime CreateDate { get; set; }
        public byte[] Timestamp { get; set; }
        public int MessageTypeId { get; set; }
        public int MessageStatusTypeId { get; set; }
        public string ToUserId { get; set; }

        public virtual MessageStatusTypeViewModel MessageStatusType { get; set; }
        public virtual MessageTypeViewModel MessageType { get; set; }
        public virtual UserViewModel User { get; set; }
        public virtual UserViewModel User1 { get; set; }
    }
}
