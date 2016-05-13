using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.ViewModel.Feed
{
   public class FeedLikeViewModel
    {
        public long FeedLikeId { get; set; }
        public long FeedId { get; set; }
        public string UserId { get; set; }

        public virtual FeedViewModel Feed { get; set; }
        public virtual UserViewModel User { get; set; }
    }
}
