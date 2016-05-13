using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.ViewModel.Feed
{
    public class UserFollowViewModel
    {
        public string UserFollowId { get; set; }
        public string UserId { get; set; }
        public DateTime FollowDate { get; set; }
        public byte[] Timestamp { get; set; }

        public virtual UserViewModel User { get; set; }
        public virtual UserViewModel User1 { get; set; }
    }
}
