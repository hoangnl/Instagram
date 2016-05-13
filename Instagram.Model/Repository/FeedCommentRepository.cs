using Instagram.Model.Common;
using Instagram.Model.EDM;
using Instagram.Model.Repository;

namespace Instagram.Model.UnitOfWork
{
    public class FeedCommentRepository : BaseRepository<FeedComment>, IFeedCommentRepository
    {
        public FeedCommentRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {

        }
    }
}