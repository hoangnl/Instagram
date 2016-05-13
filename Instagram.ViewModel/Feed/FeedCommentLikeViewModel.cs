using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.ViewModel.Feed
{
    public class FeedCommentLikeViewModel
    {
        public string UserId { get; set; }
        public long FeedCommentLikeId { get; set; }
        public long FeedCommentId { get; set; }

        public FeedCommentLikeSummary FeedCommentLikeSummary { get; set; }

        public virtual FeedCommentViewModel FeedComment { get; set; }
        public virtual UserViewModel User { get; set; }
    }
}
