using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.ViewModel.Feed
{
    public class FeedViewModel
    {
        public long FeedId { get; set; }
        public string UserId { get; set; }
        public System.DateTime CreatedTime { get; set; }
        public string Caption { get; set; }
        public string PhotoLink { get; set; }

        public virtual UserViewModel User { get; set; }

        public FeedLikeSummary FeedLikeSummary { get; set; }
        public virtual ICollection<FeedCommentViewModel> FeedComments { get; set; }
        public virtual ICollection<FeedLikeViewModel> FeedLikes { get; set; }

        public virtual ICollection<FileViewModel> Files { get; set; }

    }
}
