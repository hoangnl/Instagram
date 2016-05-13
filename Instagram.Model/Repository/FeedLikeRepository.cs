using Instagram.Model.Common;
using Instagram.Model.EDM;
using Instagram.Model.Repository;

namespace Instagram.Model.UnitOfWork
{
    public class FeedLikeRepository : BaseRepository<FeedLike>, IFeedLikeRepository
    {
        public FeedLikeRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {

        }
    }
}