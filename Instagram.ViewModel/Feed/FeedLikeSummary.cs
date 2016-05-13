using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.ViewModel.Feed
{
    public class FeedLikeSummary
    {
        public bool Liked { get; set; }
        public long FeedId { get; set; }
        public int TotalLike { get; set; }
    }
}
