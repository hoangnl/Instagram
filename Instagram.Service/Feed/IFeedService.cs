using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Instagram.Model.EDM;
using Instagram.ViewModel.Feed;

namespace Instagram.Service.Feed
{
    public interface IFeedService
    {
        IEnumerable<FeedViewModel> GetAllFeeds();

        bool SaveFeedAndFile(FeedViewModel feed, IEnumerable<FileViewModel> files);
        IEnumerable<FeedViewModel> GetFeedsPaging(string userId, int pageIndex, int pageSize);
        bool LikeFeed(string userId, long feedId);
        bool UnlikeFeed(string userId, long feedId);
        int GetFeedTotalLike(long feedId);
        FeedCommentViewModel Comment(long feedId, string userId, string content);
        bool DeleteComment(long commentId);
    }
}
