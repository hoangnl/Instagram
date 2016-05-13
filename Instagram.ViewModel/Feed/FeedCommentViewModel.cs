using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.ViewModel.Feed
{
    public class FeedCommentViewModel
    {
        public long FeedCommentId { get; set; }
        public long FeedId { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
        public System.DateTime CreatedTime { get; set; }

        public virtual FeedViewModel Feed { get; set; }
        public virtual UserViewModel User { get; set; }

        public virtual ICollection<FeedCommentLikeViewModel> FeedCommentLikes { get; set; }
    }
}
