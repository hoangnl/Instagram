using Instagram.Model.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.Model.UnitOfWork
{
    public interface IUnitOfWork
    {
        IFeedRepository FeedRepository { get; }

        IUserRepository UserRepository { get; }

        IFileRepository FileRepository { get; }
        IFeedLikeRepository FeedLikeRepository { get; }
        IFeedCommentRepository FeedCommentRepository { get; }

        IUserFollowRepository UserFollowRepository { get; }

        IAspNetUserRepository AspNetUserRepository { get; }

        bool Commit();
    }
}
