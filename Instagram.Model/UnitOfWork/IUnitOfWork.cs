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

        IUserRepository UserRepository { get; set; }

        IFileRepository FileRepository { get; }
        IFeedLikeRepository FeedLikeRepository { get; }
        IFeedCommentRepository FeedCommentRepository { get; }

        IUserFollowRepository UserFollowRepository { get; }

        IAspNetUserRepository AspNetUserRepository { get; }

        IMessageRepository MessageRepository { get; set; }

        IMessageTypeRepository MessageTypeRepository { get; }

        IMessageStatusTypeRepository MessageStatusTypeRepository { get; }

        bool Commit();
    }
}
